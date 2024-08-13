using Api.Business_Objects;
using Api.Entities;
using AutoMapper;
using Business.Services;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SegmentRectangleIntersection.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SegmentRectangleIntersection.Services
{
    public class RectangleService : IRectangleService
    {
        private readonly ICalculation _calculation;
        private readonly IMapper _mapper;
        private readonly IStorage _storage;
        private readonly ILogger<RectangleService> _logger;

        private const bool ForceNonParallel = false;
        private readonly ParallelOptions _parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = ForceNonParallel ? 1 : -1 };

        public RectangleService(ICalculation calculation, IMapper mapper, IStorage storage, ILogger<RectangleService> logger)
        {
            _calculation = calculation;
            _mapper = mapper;
            _storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// Get rectangles that intersect line segment with given coordinates.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Rectangles that have intersection with line.</returns>
        public async Task<Result<IEnumerable<Rectangle>>> GetRectanglesByCoordinatesAsync(Coordinate[] coordinates, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Trying to get rectangles that intersect line segment.");

            if (coordinates.Length != 2)
            {
                return Result.FailAndLog(new WrongDimensionsNumberError(coordinates), _logger);
            }

            var rectangleEntities = await _storage.GetAllRectanglesAsync(cancellationToken);

            if (!rectangleEntities.Any())
            {
                return Result.FailAndLog(new NoRectanglesFoundError(), _logger);
            }

            var rectangles = _mapper.Map<IEnumerable<Rectangle>>(rectangleEntities);

            var intersectedRectangles = GetIntersections(rectangles, coordinates);

            if (!intersectedRectangles.Any())
            {
                return Result.FailAndLog(new NoIntersectionFoundError(coordinates), _logger);
            }

            _logger.LogInformation("Found intersecting rectangles: {@rectangles}.", intersectedRectangles);

            return Result.Success(intersectedRectangles);
        }

        public async Task<Result> AddRectangleAsync(Rectangle rectangle, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trying to add rectangle to the storage.");

            var rectangleEntity = _mapper.Map<RectangleEntity>(rectangle);
            
            await _storage.AddRectangle(rectangleEntity, cancellationToken);

            _logger.LogInformation("Successfully added rectnagle to the storage.");

            return Result.Success();
        }

        public async Task<Result> ClearAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trying to clear the storage.");

            await _storage.ClearAsync(cancellationToken);

            _logger.LogInformation("Storaged was cleared successfully.");

            return Result.Success();
        }

        private IEnumerable<Rectangle> GetIntersections(IEnumerable<Rectangle> rectangles, Coordinate[] coordinates)
        {
            bool hasIntersection = false;

            var concurrentBag = new ConcurrentBag<Rectangle>();

            var parallelResult = Parallel.ForEach(rectangles, _parallelOptions, rectangle =>
            {
                var top = rectangle.Y + rectangle.Height;
                var right = rectangle.X + rectangle.Width;

                hasIntersection = _calculation.HasIntersection(rectangle.X, rectangle.Y, right, top, coordinates[0].X, coordinates[0].Y, coordinates[1].X, coordinates[1].Y);

                if (hasIntersection)
                {
                    concurrentBag.Add(rectangle);
                }
            });

            return concurrentBag;
        }
    }
}
