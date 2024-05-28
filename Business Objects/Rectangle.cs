using MongoDB.Bson;
using System;

namespace Api.Business_Objects
{
    public struct Rectangle
    {
        public ObjectId Id { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
    }
}
