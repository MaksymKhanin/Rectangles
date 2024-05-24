using MongoDB.Driver;
using SegmentRectangleIntersection.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SegmentRectangleIntersection.Services
{
    public class RectangleService : IRectangleService
    {
        private readonly ICalculation _calculation;
        private readonly IMongoCollection<Rectangle> _collection;

        public RectangleService(ICalculation calculation, IMongoDatabase database)
        {
            _calculation = calculation;
            _collection = database.GetCollection<Rectangle>("Rectangles");
        }
        

        //private static List<Rectangle> InMemoryStorage = new List<Rectangle> { new Rectangle(0, 0, 2, 2), new Rectangle(0, 0, 3, 3) };

        public Result<IEnumerable<Rectangle>> GetRectangle(Coordinate[] coordinates)
        {
            if (coordinates.Length != 2)
            {
                return new WrongDimensionsNumberError(coordinates);
            }

            //make it async
            var rectangles = _collection.Find(_ => true).ToList();

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

            foreach (var rectangle in rectangles)
            {
                hasIntersection = _calculation.HasIntersection(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, coordinates[0].X, coordinates[0].Y, coordinates[1].X, coordinates[1].Y);

                if (hasIntersection)
                {
                    yield return rectangle;
                }
            }
        }
    }
}
