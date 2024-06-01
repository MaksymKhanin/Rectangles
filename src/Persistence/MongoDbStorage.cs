using Api.Entities;
using Business.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class MongoDbStorage : IStorage
    {
        private readonly IMongoCollection<RectangleEntity> _collection;
        public MongoDbStorage(IMongoDatabase database) => _collection = database.GetCollection<RectangleEntity>("Rectangles");
        public async Task<IEnumerable<RectangleEntity>> GetAllRectanglesAsync(CancellationToken cancellationToken) => await _collection.Find(_ => true).ToListAsync(cancellationToken);
    }
}
