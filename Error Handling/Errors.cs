namespace SegmentRectangleIntersection.Models
{
    public record Error(ErrorCode errorCode, string message);

    public record NoIntersectionFoundError(Coordinate[] coordinates) : Error(ErrorCode.NoIntersectionFound, $"No intersection was found for Point a: '{coordinates[0].X} , {coordinates[0].Y}' and Point b: '{coordinates[1].X} , {coordinates[1].Y}'");
    public record WrongDimensionsNumberError(Coordinate[] coordinates) : Error(ErrorCode.WrongDimensionsNumber, $"Not supported dimensions number. Expected number of dimensions: 2. Found: '{coordinates.Length}'");
    public record RectangleIsEmptyError() : Error(ErrorCode.RectangleIsEmpty, "Rectangle is empty");

    public enum ErrorCode
    {
        UnknownError = 0,
        NoIntersectionFound = 1,
        RectangleIsEmpty = 2,
        WrongDimensionsNumber = 3
    }
}
