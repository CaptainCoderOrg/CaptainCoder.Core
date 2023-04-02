using System.Text;

public class DungeonGrid
{
    private Dictionary<Position, ITile> _tiles = new();
    private Dictionary<WallPosition, IWall> _walls = new();
    public GridCell CellAt(Position p)
    {
        Direction[] directions = { Direction.North, Direction.East, Direction.South, Direction.West };
        IEnumerable<(Direction, IWall)> walls = directions.Select(direction => (direction, WallAt(p, direction)));
        return new GridCell(TileAt(p), walls);
    }
    public IWall WallAt(Position position, Direction direction) => _walls.GetValueOrDefault(new WallPosition(position, direction), Wall.NoWall);
    public ITile TileAt(Position p) => _tiles.GetValueOrDefault(p, Tile.NoTile);
    public IEnumerable<(Position, ITile)> Tiles => _tiles.ToTuples();
    public IEnumerable<(WallPosition, IWall)> Walls => _walls.ToTuples();

    public void SetWall(Position position, Direction direction, Wall wall) => _walls[new WallPosition(position, direction)] = wall;
    public void DeleteWall(Position position, Direction direction) => _walls.Remove(new WallPosition(position, direction));
    public void SetTile(Position position, Tile tile) => _tiles[position] = tile;    
    public void DeleteTile(Position position) => _tiles.Remove(position);

    /// <summary>
    /// Returns the bounds of positions that could potentially have tiles.
    /// Any position outside of these bounds is guaranteed to not be passable.
    /// </summary>
    public (Position topLeft, Position bottomRight) TileBounds =>
        Position.FindMinMax(_tiles.Keys, _walls.Keys.Select(wp => wp.Position));
    public Dictionary<Position, char> ToASCII()
    {
        (Position topLeft, Position bottomRight) = TileBounds;
        Console.WriteLine($"{topLeft} - {bottomRight}");
        Position offset = topLeft;
        (int asciiRows, int asciiCols) = (bottomRight - topLeft) + (1, 1);
        Dictionary<Position, char> ascii = new ();
        foreach ((Position pos, ITile tile) in Tiles)
        {
            ascii[(pos - offset).ToASCIIPosition()] = tile.Symbol;
        }
        foreach ((WallPosition wPos, IWall wall) in Walls)
        {
            WallPosition offWall = new WallPosition(wPos.Position - offset, wPos.Direction);
            // Console.WriteLine($"Without offset: {wPos.Position}");
            // Console.WriteLine($"With offset: {offWall.Position}");
            ascii[offWall.ToASCIIPosition()] = wall.Symbol;
        }
        return ascii;
    }
}



public static class DungeonExtensions
{

    public static Position ToASCIIPosition(this Position position) => (position.Row * 2 + 1, position.Col * 2 + 1);
    public static Position ToASCIIPosition(this WallPosition wallPosition) =>
        wallPosition.Direction switch
        {
            Direction.East => (wallPosition.Position.Row * 2 + 1, wallPosition.Position.Col * 2 + 2),
            Direction.West => (wallPosition.Position.Row * 2 + 1, wallPosition.Position.Col * 2),
            Direction.North => (wallPosition.Position.Row * 2, wallPosition.Position.Col * 2 + 1),
            Direction.South => (wallPosition.Position.Row * 2 + 2, wallPosition.Position.Col * 2 + 1),
            _ => throw new NotImplementedException()
        };

    public static string ToGridString(this char[,] grid)
    {
        StringBuilder builder = new();
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                builder.Append(grid[row, col]);
            }
            builder.Append("\n");
        }
        return builder.ToString();
    }

}