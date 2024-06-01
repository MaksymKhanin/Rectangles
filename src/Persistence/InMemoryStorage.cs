using Api.Entities;
using Business.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class InMemoryStorage : IStorage
    {
        private static HashSet<RectangleEntity> InMemory = new HashSet<RectangleEntity> { new RectangleEntity(0, 0, 2, 2), new RectangleEntity(0, 0, 3, 3) };

        public async Task AddRectangle(RectangleEntity rectangleEntity, CancellationToken cancellationToken) => await Task.FromResult(InMemory.Add(rectangleEntity));

        public async Task<IEnumerable<RectangleEntity>> GetAllRectanglesAsync(CancellationToken cancellationToken) => await Task.FromResult(InMemory);
    }
}