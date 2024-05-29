using Api.Business_Objects;
using Api.Entities;
using AutoMapper;
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
        private readonly IMongoCollection<RectangleEntity> _collection;
        private readonly IMapper _mapper;


        private const bool ForceNonParallel = false;
        private readonly ParallelOptions _parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = ForceNonParallel ? 1 : -1 };

        public RectangleService(ICalculation calculation, IMongoDatabase database, IMapper mapper)
        {
            _calculation = calculation;
            _collection = database.GetCollection<RectangleEntity>("Rectangles");
            _mapper = mapper;
        }


        //private static List<Rectangle> InMemoryStorage = new List<Rectangle> { new Rectangle(0, 0, 2, 2), new Rectangle(0, 0, 3, 3) };

        public async Task<Result<IEnumerable<Rectangle>>> GetRectangle(Coordinate[] coordinates, CancellationToken cancellationToken)
        {
            if (coordinates.Length != 2)
            {
                return new WrongDimensionsNumberError(coordinates);
            }

            var rectangleEntities = await _collection.Find(_ => true).ToListAsync(cancellationToken);

            if (!rectangleEntities.Any())
            {
                return new NoRectanglesFoundError();
            }

            var rectangles = _mapper.Map<List<RectangleEntity>, List<Rectangle>>(rectangleEntities);

            var intersectedRectangles = GetIntersections(rectangles, coordinates);

            if (!intersectedRectangles.Any())
            {
                return new NoIntersectionFoundError(coordinates);
            }

            return Result.Success(intersectedRectangles);
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
