public readonly struct WallPosition
{
    public readonly Position Position;
    public readonly Direction Direction;
    private readonly int _hash;
    private readonly (Position position, Direction direction) _opposite;
    public bool IsNorthSouth => Direction == Direction.North || Direction == Direction.South;
    public WallPosition(Position position, Direction direction)
    {
        (Position, Direction) = (position, direction);
        _opposite = InitOpposite();
        _hash = HashCode.Combine(Position.GetHashCode() + _opposite.position.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        return obj is WallPosition position &&
               ((Position.Equals(position.Position) && Direction == position.Direction) ||
                (_opposite.position.Equals(position.Position) && _opposite.direction == position.Direction));
    }

    private (Position position, Direction direction) InitOpposite() =>
        Direction switch
    {
        Direction.East => (Position + (0, 1), Direction.West),
        Direction.West => (Position - (0, 1), Direction.East),
        Direction.North => (Position - (1, 0), Direction.South),
        Direction.South => (Position + (1, 0), Direction.North),
        _ => throw new NotImplementedException(),
    };


    public WallPosition Opposite => new WallPosition(_opposite.position, _opposite.direction);

    public override int GetHashCode() => _hash;
}