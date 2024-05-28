using Api.Business_Objects;
using SegmentRectangleIntersection.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SegmentRectangleIntersection.Services
{
    public interface IRectangleService
    {
        Task<Result<IEnumerable<Rectangle>>> GetRectangle(Coordinate[] point, CancellationToken cancellationToken);
    }
}
