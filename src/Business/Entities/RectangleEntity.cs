using MongoDB.Bson;

namespace Api.Entities
{
    public struct RectangleEntity
    {
        public ObjectId Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }
}
