using System;
using System.Linq;
using CaptainCoder.Core;
namespace CaptainCoder.Dice;

/// <summary>
/// A <see cref="DiceGroup"/> represents a group of like dice to be rolled together (e.g. 3d6 is 3 six-sided dice)
/// </summary>
public record DiceGroup
{
    private readonly IRandom _rng;
    private DiceGroup(int count, int sides, IRandom randomSource)
    {
        if (count < 1) { throw new ArgumentException($"{nameof(count)} must be positive but was {count}."); }
        if (sides < 2) { throw new ArgumentException($"{nameof(sides)} must be 2 or greater but was {sides}."); }
        Count = count;
        Sides = sides;
        _rng = randomSource ?? throw new ArgumentNullException($"{nameof(randomSource)} must be non-null.");
    }

    /// <summary>
    /// The number of dice in this group
    /// </summary>
    public int Count { get; }
    /// <summary>
    /// The number of sides on each die in this group.
    /// </summary>
    public int Sides { get; }
    /// <summary>
    /// The minimum possible value this group can roll
    /// </summary>
    public int Min => Count;
    /// <summary>
    /// The maximum possible value this group can roll
    /// </summary>
    public int Max => Count * Sides;
    /// <summary>
    /// The dice notation of this group.
    /// </summary>
    public string Notation => $"{Count}d{Sides}";

    /// <summary>
    /// Rolls this dice group.
    /// </summary>
    public RollResult Roll() => Roll(_rng);
    /// <summary>
    /// Rolls this dice group specifying the source of randomness.
    /// </summary>
    public RollResult Roll(IRandom randomSource) 
    {
        int RollDie() => randomSource.Next(1, Sides + 1);
        var dice = Enumerable.Repeat(RollDie, Count).Select(roll => roll());
        int[] rolls = dice.ToArray();
        int total = rolls.Sum();
        string message = $"{Count}d{Sides} ({string.Join(" + ", dice)} = {total})";
        return new RollResult(message, total);
    }

    /// <summary>
    /// Attempts to parse a dice group of the format "{count}d{sides}". The
    /// count must be positive and the sides must be greater than 1.
    /// </summary>
    public static bool TryParse(string notation, out DiceGroup result) => TryParse(notation, IRandom.Shared, out result);

    /// <summary>
    /// Attempts to parse a dice group of the format "{count}d{sides}" while
    /// specifying the source of randomness. The count must be positive and the
    /// sides must be greater than 1.
    /// </summary>
    public static bool TryParse(string notation, IRandom randomSource, out DiceGroup result)
    {
        if (notation == null) { throw new ArgumentNullException($"{nameof(notation)} must be non-null."); }
        if (randomSource == null) { throw new ArgumentNullException($"{nameof(randomSource)} must be non-null."); }
        result = null!;
        string[] split = notation.Split('d');
        if (split.Length != 2) { return false; }
        if (!int.TryParse(split[0], out int count)) { return false; }
        if (!int.TryParse(split[1], out int sides)) { return false; }
        if (count < 1) { return false; }
        if (sides < 2) { return false; }
        result = new DiceGroup(count, sides, randomSource);
        return true;
    }
}