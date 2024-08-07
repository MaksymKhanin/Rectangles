﻿using Api.Entities;
using Business.Services;
using MongoDB.Bson;
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
        public async Task AddRectangle(RectangleEntity rectangleEntity, CancellationToken cancellationToken) => await _collection.InsertOneAsync(rectangleEntity, null, cancellationToken);
        public async Task ClearAsync(CancellationToken cancellationToken) => await _collection.DeleteManyAsync(new BsonDocument());
    }
}
