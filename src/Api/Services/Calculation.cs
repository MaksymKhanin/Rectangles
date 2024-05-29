using System;

namespace SegmentRectangleIntersection.Services
{
    public class Calculation : ICalculation
    {
        public bool HasIntersection(double rectangleMinX, double rectangleMinY, double rectangleMaxX, double rectangleMaxY, double point1X, double point1Y, double point2X, double point2Y)
        {
            // Minimum and maximum x for line segment

            var mimimumX = point1X;
            var maximumX = point2X;

            if (point1X > point2X)
            {
                mimimumX = point2X;
                maximumX = point1X;
            }

            // Intersections of x projections of line segment and rect.

            if (mimimumX < rectangleMinX)
            {
                mimimumX = rectangleMinX;
            }

            if (maximumX > rectangleMaxX)
            {
                maximumX = rectangleMaxX;
            }

            //in case of no intersection, return

            if (mimimumX > maximumX)
            {
                return false;
            }

            // Minimum and maximum Y for minimum and maximum X that were found in previous step

            var minimumY = point1Y;
            var maximumY = point2Y;

            var distanceX = point2X - point1X;

            if (Math.Abs(distanceX) > 0.0000001)
            {
                var slopeA = (point2Y - point1Y) / distanceX;
                var slopeB = point1Y - slopeA * point1X;

                minimumY = slopeA * mimimumX + slopeB;
                maximumY = slopeA * maximumX + slopeB;
            }

            if (minimumY > maximumY)
            {
                var tempVar = maximumY;

                maximumY = minimumY;
                minimumY = tempVar;
            }

            // Intersections of y projections of line segment and rect.

            if (minimumY < rectangleMinY)
            {
                minimumY = rectangleMinY;
            }

            if (maximumY > rectangleMaxY)
            {
                maximumY = rectangleMaxY;
            }

            //in case of no intersection, return

            if (minimumY > maximumY)
            {
                return false;
            }

            return true;
        }
    }
}
