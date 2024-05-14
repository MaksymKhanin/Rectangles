﻿using SegmentRectangleIntersection.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SegmentRectangleIntersection.Services
{
    public class RectangleService : IRectangleService
    {
        private readonly ICalculation _calculation;
        public RectangleService(ICalculation calculation) => _calculation = calculation;

        private static List<Rectangle> InMemoryStorage = new List<Rectangle> { new Rectangle(0, 0, 2, 2), new Rectangle(0, 0, 3, 3) };

        public Result<IEnumerable<Rectangle>> GetRectangle(Coordinate[] coordinates)
        {
            if (coordinates.Length != 2)
            {
                return new WrongDimensionsNumberError(coordinates);
            }

            var intersectedRectangles = GetIntersections(InMemoryStorage, coordinates);

            if (!intersectedRectangles.Any())
            {
                return new NoIntersectionFoundError(coordinates);
            }

            return Result.Success(intersectedRectangles);
        }

        public Result Add(Rectangle rec)
        {
            if (rec.IsEmpty)
            {
                return new RectangleIsEmptyError();
            }

            InMemoryStorage.Add(rec);

            return Result.Success();
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
