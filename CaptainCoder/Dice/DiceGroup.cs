using System;
using System.Linq;
using CaptainCoder.Core;
namespace CaptainCoder.Dice;
public record DiceGroup : IRollable
{
    private IRandom _rng;
    public DiceGroup(int count, int sides, IRandom randomSource)
    {
        if (count < 1) { throw new ArgumentException($"{nameof(count)} must be positive but was {count}."); }
        if (sides < 2) { throw new ArgumentException($"{nameof(sides)} must be 2 or greater but was {sides}."); }
        Count = count;
        Sides = sides;
        _rng = randomSource ?? throw new ArgumentNullException($"{nameof(randomSource)} must be non-null.");
    }

    public DiceGroup(int count, int sides) : this(count, sides, IRandom.Shared) {}
    public int Count { get; }
    public int Sides { get; }
    public int Min => Count;
    public int Max => Count * Sides;
    public string Standardized => $"{Count}d{Sides}";
    public RollResult Roll() => Roll(_rng);
    public RollResult Roll(IRandom rng) 
    {
        int RollDie() => rng.Next(1, Sides + 1);
        var dice = Enumerable.Repeat(RollDie, Count).Select(roll => roll());
        int[] rolls = dice.ToArray();
        int total = rolls.Sum();
        string message = $"{Count}d{Sides} ({string.Join(" + ", dice)} = {total})";
        return new RollResult(message, total);
    }

    public static bool TryParse(string notation, out DiceGroup result)
    {
        result = null!;
        string[] split = notation.Split('d');
        if (split.Length != 2) { return false; }
        if (!int.TryParse(split[0], out int count)) { return false; }
        if (!int.TryParse(split[1], out int sides)) { return false; }
        if (count < 1) { return false; }
        if (sides < 2) { return false; }
        result = new DiceGroup(count, sides);
        return true;
    }
}