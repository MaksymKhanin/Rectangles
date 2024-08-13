using Api.Business_Objects;
using SegmentRectangleIntersection.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SegmentRectangleIntersection.Services
{
    public interface IRectangleService
    {
        Task<Result> AddRectangleAsync(Rectangle rectangle, CancellationToken cancellationToken);
        Task<Result> ClearAsync(CancellationToken cancellationToken);
        Task<Result<IEnumerable<Rectangle>>> GetRectanglesByCoordinatesAsync(Coordinate[] point, CancellationToken cancellationToken = default);
    }
}
