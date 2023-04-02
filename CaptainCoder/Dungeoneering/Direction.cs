public enum Direction
{
    North, East, South, West
}

public static class DirectionExtensions
{
    public static Direction RotateClockwise(this Direction direction) => (Direction)(((int)direction + 1) % 4);
    public static Direction RotateCounterClockwise(this Direction direction) => (Direction)(((int)direction + 3) % 4);
    public static Position MovePosition(this Direction direction) => direction switch
    {
        Direction.North => (-1, 0),
        Direction.South => (1, 0),
        Direction.East => (0, 1),
        Direction.West => (0, -1),
        _ => throw new NotImplementedException(),
    };
}