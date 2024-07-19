using Api.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IStorage
    {
        Task AddRectangle(RectangleEntity rectangleEntity, CancellationToken cancellationToken);
        Task ClearAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<RectangleEntity>> GetAllRectanglesAsync(CancellationToken cancellationToken);
    }
}
