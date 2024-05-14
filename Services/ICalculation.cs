namespace SegmentRectangleIntersection.Services
{
    public interface ICalculation
    {
        bool HasIntersection(double rectangleMinX, double rectangleMinY, double rectangleMaxX, double rectangleMaxY, double point1X, double point1Y, double point2X, double point2Y);
    }
}
