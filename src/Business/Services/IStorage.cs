using Api.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IStorage
    {
        public Task<IEnumerable<RectangleEntity>> GetAllRectanglesAsync(CancellationToken cancellationToken);
    }
}
