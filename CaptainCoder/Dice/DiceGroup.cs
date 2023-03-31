using System;
using System.Linq;
using CaptainCoder.Core;
using Sprache;
namespace CaptainCoder.Dice;

/// <summary>
/// A <see cref="DiceGroup"/> represents a group of like dice to be rolled together (e.g. 3d6 is 3 six-sided dice)
/// </summary>
public record DiceGroup
{
    private IRandom _rng;
    internal DiceGroup(int count, int sides, IRandom randomSource)
    {
        if (count < 1) { throw new ArgumentException($"{nameof(count)} must be positive but was {count}."); }
        if (sides < 2) { throw new ArgumentException($"{nameof(sides)} must be 2 or greater but was {sides}."); }
        Count = count;
        Sides = sides;
        _rng = randomSource ?? throw new ArgumentNullException($"{nameof(randomSource)} must be non-null.");
    }

    internal DiceGroup(int count, int sides) : this(count, sides, IRandom.Shared) { }

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
        int[] rolls = Enumerable.Repeat(RollDie, Count).Select(roll => roll()).ToArray();
        int total = rolls.Sum();
        string message = $"{Count}d{Sides} ({string.Join(" + ", rolls)} = {total})";
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
        IResult<DiceGroup> parseResult = Parsers.DiceGroup.TryParse(notation);
        if(parseResult.WasSuccessful)
        {
            result = parseResult.Value;
            result._rng = randomSource ?? throw new ArgumentNullException($"{nameof(notation)} must be non-null.");
            return true;
        }
        result = null!;
        return false;
    }
}

