namespace CaptainCoder.Core.Dice;

public interface IRollable
{
    public int Min { get; }
    public int Max { get; }
    public RollResult Roll();
}

public interface IRollContext
{
    public int Lookup(string Id);
}