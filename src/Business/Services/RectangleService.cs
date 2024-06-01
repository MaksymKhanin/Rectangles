using Api.Business_Objects;
using Api.Entities;
using AutoMapper;
using Business.Services;
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

        private const bool ForceNonParallel = false;
        private readonly ParallelOptions _parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = ForceNonParallel ? 1 : -1 };

        public RectangleService(ICalculation calculation, IMapper mapper, IStorage storage)
        {
            _calculation = calculation;
            _mapper = mapper;
            _storage = storage;
        }

        public async Task<Result<IEnumerable<Rectangle>>> GetRectangle(Coordinate[] coordinates, CancellationToken cancellationToken = default)
        {
            if (coordinates.Length != 2)
            {
                return new WrongDimensionsNumberError(coordinates);
            }

            var rectangleEntities = await _storage.GetAllRectanglesAsync(cancellationToken);

            if (!rectangleEntities.Any())
            {
                return new NoRectanglesFoundError();
            }

            var rectangles = _mapper.Map<IEnumerable<Rectangle>>(rectangleEntities);

            var intersectedRectangles = GetIntersections(rectangles, coordinates);

            if (!intersectedRectangles.Any())
            {
                return new NoIntersectionFoundError(coordinates);
            }

            return Result.Success(intersectedRectangles);
        }

        public async Task<Result> AddRectangle(Rectangle rectangle, CancellationToken cancellationToken)
        {
            var rectangleEntity = _mapper.Map<RectangleEntity>(rectangle);
            
            await _storage.AddRectangle(rectangleEntity, cancellationToken);

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
