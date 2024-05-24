using AutoFixture.Xunit2;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using SegmentRectangleIntersection.Models;
using SegmentRectangleIntersection.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Api.Tests
{
    [Category("Unit")]
    public class RectangleServiceShould
    {
        private readonly IRectangleService Sut;
        private readonly Mock<Calculation> _calculation = new();
        private readonly Mock<IMongoDatabase> _storage = new();

        public RectangleServiceShould()
        {
            Sut = new RectangleService(_calculation.Object, _storage.Object);
        }

        //[Theory(DisplayName = "GetRectangle method should return Success and an intersecting rectangle when intersection exists")]
        //[InlineAutoData(0, 3, 3, 3, 0, 0, 3, 3)]
        //public void Test001(double x1, double y1, double x2, double y2, int expectedRectX, int expectedRectY, int expectedWidth, int expectedHeight)
        //{
        //    Coordinate[] coordinates = { new Coordinate(x1, y1), new Coordinate(x2, y2) };

        //    var actualResult = Sut.GetRectangle(coordinates);

        //    actualResult.Should().BeOfType<Result<IEnumerable<Rectangle>>>();

        //    actualResult.Value.First().Width.Should().Be(expectedWidth);
        //    actualResult.Value.First().Height.Should().Be(expectedHeight);
        //    actualResult.Value.First().X.Should().Be(expectedRectX);
        //    actualResult.Value.First().Y.Should().Be(expectedRectY);
        //}

        [Theory(DisplayName = "GetRectangle method should return error when not 2 coordinates are passed")]
        [AutoData]
        public void Test002(Coordinate c1, Coordinate c2, Coordinate c3)
        {
            Coordinate[] coordinates = { c1, c2, c3 };

            var actualResult = Sut.GetRectangle(coordinates);

            actualResult.Error.Should().BeOfType<WrongDimensionsNumberError>();
            actualResult.Error.errorCode.Should().Be(ErrorCode.WrongDimensionsNumber);
            actualResult.Error.message.Should().Be($"Not supported dimensions number. Expected number of dimensions: 2. Found: '3'");
        }

        //[Theory(DisplayName = "GetRectangle method should return error when not no intersection found")]
        //[InlineAutoData(0, 5, 5, 5, 0, 0, 3, 3)]
        //public void Test003(double x1, double y1, double x2, double y2, int expectedRectX, int expectedRectY, int expectedWidth, int expectedHeight)
        //{
        //    Coordinate[] coordinates = { new Coordinate(x1, y1), new Coordinate(x2, y2) };

        //    var actualResult = Sut.GetRectangle(coordinates);

        //    actualResult.Error.Should().BeOfType<NoIntersectionFoundError>();
        //    actualResult.Error.errorCode.Should().Be(ErrorCode.NoIntersectionFound);
        //    actualResult.Error.message.Should().Be($"No intersection was found for Point a: '{x1} , {y1}' and Point b: '{x2} , {y2}'");
        //}
    }
}
