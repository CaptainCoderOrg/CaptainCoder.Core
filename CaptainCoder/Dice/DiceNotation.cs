using System;
using System.Diagnostics;
using System.Linq;
using CaptainCoder.Core;
using Sprache;
namespace CaptainCoder.Dice;

/// <summary>
/// A <see cref="DiceNotation"/> is an arithmetic expression made up of dice groups, variables, and integers.
/// </summary>
public record DiceNotation
{
    private readonly IRollableExpr _expr;
    private readonly IRandom _rng;

    /// <summary>
    /// The notation used.
    /// </summary>
    public string Notation { get; }

    private DiceNotation(string notation, IRandom randomSource, IRollableExpr expr) => (Notation, _rng, _expr) = (notation, randomSource, expr);

    /// <summary>
    /// Rolls any dice groups contained withing this <see cref="DiceNotation"/>
    /// and evaluates the result. An <see cref="IRollContext"/> must be provided
    /// if any variables exist within this notation.
    /// </summary>
    public RollResult Roll(IRollContext context) => _expr.Eval(context, _rng);

    /// <summary>
    /// Rolls any dice groups contained withing this <see cref="DiceNotation"/>
    /// using the specified source of randomness and evaluates the result. An
    /// <see cref="IRollContext"/> must be provided if any variables exist
    /// within this notation.
    /// </summary>
    public RollResult Roll(IRollContext context, IRandom randomSource) => _expr.Eval(context, randomSource);

    /// <summary>
    /// Parses the specified <paramref name="diceNotation"/>.
    /// </summary>
    public static DiceNotation Parse(string diceNotation) => Parse(diceNotation, IRandom.Shared);

    /// <summary>
    /// Parses the specified <paramref name="diceNotation"/> specifying the
    /// source of randomness to be used on rolls.
    /// </summary>
    public static DiceNotation Parse(string diceNotation, IRandom randomSource)
    {
        if (diceNotation == null) { throw new ArgumentNullException($"{nameof(diceNotation)} must be non-null."); }
        if (randomSource == null) { throw new ArgumentNullException($"{nameof(randomSource)} must be non-null."); }
        IResult<IRollableExpr> parseResult = Parsers.Expression.TryParse(diceNotation);
        Debug.WriteLine($"Remainder: {parseResult.Remainder}");
        if (!parseResult.WasSuccessful) { throw new FormatException(parseResult.Message); }
        return new DiceNotation(diceNotation, randomSource, parseResult.Value);
    }
}