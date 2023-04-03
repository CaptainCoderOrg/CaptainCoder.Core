using System.IO;
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
    public bool HasWallsAt(Position position)
    {
        foreach (Direction d in DirectionExtensions.All)
        {
            if (_walls.ContainsKey(new WallPosition(position, d))) { return true; }
        }
        return false;
    }
    public bool HasTileAt(Position position) => _tiles.ContainsKey(position);
    public IWall WallAt(Position position, Direction direction) => _walls.GetValueOrDefault(new WallPosition(position, direction), Wall.NoWall);
    public ITile TileAt(Position p) => _tiles.GetValueOrDefault(p, Tile.NoTile);
    public IEnumerable<(Position, ITile)> Tiles => _tiles.ToTuples();
    public IEnumerable<(WallPosition, IWall)> Walls => _walls.ToTuples();

    public void SetWall(Position position, Direction direction, Wall wall) => _walls[new WallPosition(position, direction)] = wall;
    public void SetWall(WallPosition wallPosition, IWall wall)
    {
        if (wall == Wall.NoWall) { _walls.Remove(wallPosition); }
        else { _walls[wallPosition] = wall; }
    } 
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
        Dictionary<Position, char> ascii = new();
        foreach ((Position pos, ITile tile) in Tiles)
        {
            ascii[(pos).ToASCIIPosition()] = tile.Symbol;
        }
        foreach ((WallPosition wPos, IWall wall) in Walls)
        {
            ascii[wPos.ToASCIIPosition()] = wall.Symbol;
            AddNeighbors(wPos, ascii);
        }
        return ascii;
    }

    private void AddNeighbors(WallPosition wPos, Dictionary<Position, char> ascii)
    {
        if (wPos.Direction == Direction.North || wPos.Direction == Direction.South)
        {
            AddEastWestNeighbor(wPos, ascii);
        }
        else
        {
            AddNorthSouthNeighbor(wPos, ascii);
        }
    }

    private void AddEastWestNeighbor(WallPosition wPos, Dictionary<Position, char> ascii)
    {
        ascii[wPos.ToASCIIPosition() + (0, 1)] = Wall.Solid.Symbol;
        ascii[wPos.ToASCIIPosition() + (0, -1)] = Wall.Solid.Symbol;
    }

    private void AddNorthSouthNeighbor(WallPosition wPos, Dictionary<Position, char> ascii)
    {
        ascii[wPos.ToASCIIPosition() + (1, 0)] = Wall.Solid.Symbol;
        ascii[wPos.ToASCIIPosition() + (-1, 0)] = Wall.Solid.Symbol;
    }

    public static bool TryLoad(string path, out DungeonGrid grid)
    {
        grid = new DungeonGrid();
        try
        {
            string[] rows = File.ReadAllLines(path);
            for (int row = 0; row < rows.Length; row++)
            {
                for (int col = 0; col < rows[row].Length; col++)
                {
                    char ch = rows[row][col];
                    if (new Position(row, col).TryToWallPosition(out WallPosition wallPosition))
                    {
                        // Console.WriteLine($"({row}, {col}) => ({wallPosition.Position}, {wallPosition.Direction})");
                        // Console.ReadLine();
                        grid.SetWall(wallPosition, LoadWall(ch));
                    }
                    else if (new Position(row, col).TryToDungeonPosition(out Position dungeonPosition))
                    {
                        grid.SetTile(dungeonPosition, LoadTile(ch));
                    }
                }
            }
            return true;
        }
        catch (Exception e) {
            Console.Error.WriteLine(e.Message);
            Console.ReadLine();
            return false;
        }
    }

    private static IWall LoadWall(char ch)
    {
        IWall wall = ch switch
        {
            ' ' => Wall.NoWall,
            '#' => Wall.Solid,
            '+' => Wall.Door,
            _ => throw new NotImplementedException()
        };
        return wall;
    }

    public static Tile LoadTile(char ch)
    {
        return ch switch
        {
            ' ' => Tile.NoTile,
            '.' => Tile.Floor,
            (> 'A') and (< 'z') => new Tile(ch, true),
            _ => throw new NotImplementedException(),
        };
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

    public static string ToGridString(this Dictionary<Position, char> grid)
    {
        StringBuilder builder = new();
        (Position min, Position max) = Position.FindMinMax(grid.Keys);
        (int rows, int cols) = (max - min) + (1, 1);
        // char[,] ascii = new char[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Position p = (row, col) + min;
                builder.Append(grid.GetValueOrDefault(p, ' '));
            }
            builder.Append('\n');
        }
        return builder.ToString();
    }

    public static bool TryToWallPosition(this Position position, out WallPosition wallPosition)
    {
        wallPosition = default;
        if (position.Row % 2 == 0)
        {
            // Invalid position
            if (position.Col % 2 == 0) { return false; }

            // Must be North / South Wall
            Position newPos = (position.Row/2, position.Col/2);
            wallPosition = new WallPosition(newPos, Direction.North);
            return true;
        }
        else 
        {
            
            // Must be East / West Wall
            if (position.Col % 2 == 1) { return false; }
            Position newPos = (position.Row/2, position.Col/2);
            wallPosition = new WallPosition(newPos, Direction.West);
            return true;
        }
        return false;
    }

    public static bool TryToDungeonPosition(this Position position, out Position dungeonPosition)
    {
        dungeonPosition = default;
        if (position.Row % 2 == 0 || position.Col % 2 == 0) { return false; }
        dungeonPosition = new Position(position.Row / 2, position.Col / 2);
        return true;
    }

}