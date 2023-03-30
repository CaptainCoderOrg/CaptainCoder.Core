using System;
using System.Linq;
namespace CaptainCoder.Core.Dice;

public record DiceNotation
{
    private readonly IRollableExpr _expr;
    private readonly IRandom _rng;

    public string Notation { get; }

    private DiceNotation(string notation, IRandom randomSource, IRollableExpr expr) => (Notation, _rng, _expr) = (notation, randomSource, expr);
    public RollResult Roll(IRollContext context) => _expr.Eval(context);

    public static DiceNotation Parse(string diceNotation) => Parse(diceNotation, IRandom.Shared);
    public static DiceNotation Parse(string diceNotation, IRandom randomSource)
    {
        if (diceNotation == null) { throw new ArgumentNullException($"{nameof(diceNotation)} must be non-null."); }
        if (randomSource == null) { throw new ArgumentNullException($"{nameof(randomSource)} must be non-null."); }
        IRollableExpr expr = DiceNotationParser.Parse(diceNotation);
        return new DiceNotation(diceNotation, randomSource, expr);
    }
}