public readonly struct WallPosition
{
    public readonly Position Position;
    public readonly Direction Direction;
    private readonly int _hash;
    public WallPosition(Position position, Direction direction)
    {
        (Position, Direction) = (position, direction);
        _hash = HashCode.Combine(Position.GetHashCode() + Opposite.Position.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        return obj is WallPosition position &&
               ((Position.Equals(position.Position) && Direction == position.Direction) ||
                (Opposite.Position.Equals(position.Position) && Opposite.Direction == position.Direction));
    }

    public WallPosition Opposite => Direction switch
    {
        Direction.East => new WallPosition(Position - (0, 1), Direction.West),
        Direction.West => new WallPosition(Position + (0, 1), Direction.East),
        Direction.North => new WallPosition(Position + (1, 0), Direction.South),
        Direction.South => new WallPosition(Position - (1, 0), Direction.North),
        _ => throw new NotImplementedException(),
    };

    public override int GetHashCode() => _hash;
}