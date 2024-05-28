using Microsoft.AspNetCore.Mvc;
using SegmentRectangleIntersection.Models;
using SegmentRectangleIntersection.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SegmentRectangleIntersection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RectangleController : ControllerBase
    {
        private readonly IRectangleService _rectangleService;
        public RectangleController(IRectangleService rectangleService) => _rectangleService = rectangleService;

        [HttpPost("/findIntersectingRectangles")]
        public async Task<IActionResult> SendCoordinates(Coordinate[] coordinates, CancellationToken cancellationToken)
        {
            return (await _rectangleService.GetRectangle(coordinates, cancellationToken)).Match<IActionResult>(
                success => Ok(success),
                error => BadRequest(error),
                notFound => NotFound(notFound));
        }
    }
}
