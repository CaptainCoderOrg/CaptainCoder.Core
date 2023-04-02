public interface IWall
{
    public char Symbol { get; }
    public bool IsPassable { get; }
}

public class Wall : IWall
{
    public static Wall NoWall { get; } = new Wall(' ', true);
    public static Wall Solid { get; } = new Wall('#', false);
    public static Wall Door { get; } = new Door();
    public Wall(char symbol, bool isPassable) => (Symbol, IsPassable) = (symbol, isPassable);
    public char Symbol { get; protected set; }
    public bool IsPassable { get; protected set; }
}

public class Door : Wall
{
    public Door() : base('+', false) {}

    public void Open()
    {
        IsPassable = true;
        Symbol = '-';
    }

    public void Close()
    {
        IsPassable = false;
        Symbol = '+';
    }
}