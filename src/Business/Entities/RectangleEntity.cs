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

        public RectangleEntity(int x, int y, int width, int height) : this()
        {
            Id = ObjectId.GenerateNewId();
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
