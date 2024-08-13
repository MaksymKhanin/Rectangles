using Api.Business_Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SegmentRectangleIntersection.Models;
using SegmentRectangleIntersection.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SegmentRectangleIntersection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RectangleController : ControllerBase
    {
        private readonly IRectangleService _rectangleService;
        private readonly ILogger<RectangleController> _logger;
        public RectangleController(IRectangleService rectangleService, ILogger<RectangleController> logger)
        {
            _rectangleService = rectangleService
                ?? throw new ArgumentNullException(nameof(rectangleService));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("/findIntersectingRectangles")]
        public async Task<IActionResult> SendCoordinates(Coordinate[] coordinates, CancellationToken cancellationToken)
        {
            _logger.BeginScope("Request: {@request}", coordinates);
            _logger.LogInformation("Received request to find intersecting rectangles by coordinates: {@coordinates}", coordinates);

            return (await _rectangleService.GetRectanglesByCoordinatesAsync(coordinates, cancellationToken)).Match<IActionResult>(
                success => Ok(success),
                error => BadRequest(error),
                notFound => NotFound(notFound));
        }

        [HttpPost("/addRectangle")]
        public async Task<IActionResult> AddRectangle(Rectangle rectangle, CancellationToken cancellationToken)
        {
            _logger.BeginScope("Request: {@request}", rectangle);
            _logger.LogInformation("Received reuest to add rectangle: {@rectangle}", rectangle);

            var result = await _rectangleService.AddRectangleAsync(rectangle, cancellationToken);

            return (result.IsSuccess) 
                ? Ok() 
                : BadRequest(result.Error);
        }

        [HttpPost("/clear")]
        public async Task<IActionResult> Clear(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to clear storage");

            var result = await _rectangleService.ClearAsync(cancellationToken);

            return (result.IsSuccess)
                 ? Ok()
                 : BadRequest(result.Error);
        }
    }
}
