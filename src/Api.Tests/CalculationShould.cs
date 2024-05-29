using AutoFixture.Xunit2;
using FluentAssertions;
using SegmentRectangleIntersection.Services;
using System.ComponentModel;
using Xunit;

namespace Api.Tests
{
    [Category("Unit")]
    public class CalculationShould
    {
        private readonly Calculation Sut;

        public CalculationShould() => Sut = new Calculation();

        [Theory(DisplayName = "HasIntersection method should return true when intersection exists")]
        [InlineAutoData(0, 0, 4, 4, 0, 4, 4, 4, true)]
        [InlineAutoData(-1, 0, 4, 4, -1, 0, -1, 6, true)]
        [InlineAutoData(0, 0, 4, 4, -1, 4, 4, 4, true)]
        [InlineAutoData(-1, 0, 4, 4, -1, -2, -1, 6, true)]
        public void Test001(double rectangleMinX, double rectangleMinY, double rectangleMaxX, double rectangleMaxY, double point1X, double point1Y, double point2X, double point2Y, bool expectedResult)
        {
            var actualResukt = Sut.HasIntersection(rectangleMinX, rectangleMinY, rectangleMaxX, rectangleMaxY, point1X, point1Y, point2X, point2Y);

            actualResukt.Should().Be(expectedResult);
        }

        [Theory(DisplayName = "HasIntersection method should return false when no intersection exists")]
        [InlineAutoData(0, 0, 3, 3, 0, 4, 4, 4, false)]
        [InlineAutoData(-1, 0, 4, 4, -1, 5, -1, 6, false)]
        [InlineAutoData(0, 0, 3, 3, 0, 5, 4, 6, false)]
        [InlineAutoData(-1, 0, 4, 4, -2, 8, -1, 6, false)]
        public void Test002(double rectangleMinX, double rectangleMinY, double rectangleMaxX, double rectangleMaxY, double point1X, double point1Y, double point2X, double point2Y, bool expectedResult)
        {
            var actualResukt = Sut.HasIntersection(rectangleMinX, rectangleMinY, rectangleMaxX, rectangleMaxY, point1X, point1Y, point2X, point2Y);

            actualResukt.Should().Be(expectedResult);
        }

        [Theory(DisplayName = "HasIntersection method should return true when segment is inside rectangle")]
        [InlineAutoData(0, 0, 4, 4, 1, 1, 3, 3, true)]
        [InlineAutoData(0, 0, 4, 4, 2, 3, 2, 3, true)]
        public void Test003(double rectangleMinX, double rectangleMinY, double rectangleMaxX, double rectangleMaxY, double point1X, double point1Y, double point2X, double point2Y, bool expectedResult)
        {
            var actualResukt = Sut.HasIntersection(rectangleMinX, rectangleMinY, rectangleMaxX, rectangleMaxY, point1X, point1Y, point2X, point2Y);

            actualResukt.Should().Be(expectedResult);
        }
    }
}
