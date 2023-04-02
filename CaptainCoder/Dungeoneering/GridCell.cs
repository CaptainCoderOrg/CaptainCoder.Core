public class GridCell
{
    private readonly Dictionary<Direction, IWall> _walls;
    public GridCell(ITile tile, IEnumerable<(Direction, IWall)> walls)
    {
        Tile = tile;
        _walls= walls.ToDictionary();
        Walls = _walls.ToTuples();
    }
    public ITile Tile { get; }
    public IEnumerable<(Direction, IWall)> Walls { get; }
    public IWall WallAt(Direction direction) => _walls[direction];
}