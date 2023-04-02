public interface ITile
{
    public char Symbol { get; }
    public bool IsPassable { get; }
}

public class Tile : ITile
{
    public static Tile NoTile { get; } = new Tile(' ', false);
    public Tile (char symbol, bool isPassable) => (Symbol, IsPassable) = (symbol, isPassable);
    public char Symbol { get; }
    public bool IsPassable { get; }
}