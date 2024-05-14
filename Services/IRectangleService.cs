using System.Collections.Generic;
using System.Drawing;
using SegmentRectangleIntersection.Models;
using static SegmentRectangleIntersection.Controllers.RectangleController;

namespace SegmentRectangleIntersection.Services
{
    public interface IRectangleService
    {
        Result<IEnumerable<Rectangle>> GetRectangle(Coordinate[] point);
        Result Add(Rectangle rec);
    }
}
