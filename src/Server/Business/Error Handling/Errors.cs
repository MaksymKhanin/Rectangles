namespace SegmentRectangleIntersection.Models
{
    public record Error(ErrorCode ErrorCode, string Message);

    public record NotFoundError(ErrorCode ErrorCode, string Message) : Error(ErrorCode, Message) { public NotFoundError() : this(ErrorCode.NotFoundError, "No Data was found") { } }
    public record NoIntersectionFoundError(Coordinate[] coordinates) : NotFoundError(ErrorCode.NoIntersectionFound, $"No intersection was found for Point a: '{coordinates[0].X} , {coordinates[0].Y}' and Point b: '{coordinates[1].X} , {coordinates[1].Y}'");
    public record WrongDimensionsNumberError(Coordinate[] coordinates) : Error(ErrorCode.WrongDimensionsNumber, $"Not supported dimensions number. Expected number of dimensions: 2. Found: '{coordinates.Length}'");
    public record NoRectanglesFoundError() : NotFoundError(ErrorCode.NoRectanglesFoundError, "No data was found in the storage.");
    public record RectangleIsEmptyError() : Error(ErrorCode.RectangleIsEmpty, "Rectangle is empty");

    public enum ErrorCode
    {
        UnknownError = 0,
        NotFoundError = 1,
        NoIntersectionFound = 2,
        RectangleIsEmpty = 3,
        WrongDimensionsNumber = 4,
        NoRectanglesFoundError = 5
    }
}
