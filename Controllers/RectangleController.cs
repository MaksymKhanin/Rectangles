using Microsoft.AspNetCore.Mvc;
using SegmentRectangleIntersection.Models;
using SegmentRectangleIntersection.Services;
using System.Drawing;

namespace SegmentRectangleIntersection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RectangleController : ControllerBase
    {
        private readonly IRectangleService _rectangleService;
        public RectangleController(IRectangleService rectangleService) => _rectangleService = rectangleService;

        [HttpPost("/findIntersectingRectangles")]
        public IActionResult SendCoordinates(Coordinate[] coordinates)
        {
            return _rectangleService.GetRectangle(coordinates).Match<IActionResult>(
                success => Ok(success),
                error => BadRequest(error),
                notFound => NotFound(notFound));
        }


        [HttpPost("/add")]
        public IActionResult Add(Rectangle rec) => _rectangleService.Add(rec).Match<IActionResult>(
                () => Accepted(),
                error => BadRequest(error));
    }
}
