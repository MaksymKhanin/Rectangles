﻿using Api.Entities;
using Business.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class InMemoryStorage : IStorage
    {
        private static List<RectangleEntity> InMemory = new List<RectangleEntity> { new RectangleEntity(0, 0, 2, 2), new RectangleEntity(0, 0, 3, 3) };

        public Task<List<RectangleEntity>> GetAllRectanglesAsync(CancellationToken cancellationToken) => Task.FromResult(InMemory);
    }
}
