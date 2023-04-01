using System.Collections.Generic;
using System.Linq;
using Sprache;
namespace CaptainCoder.Dice;

// Dice Notation Grammar:
// Expr: $ArithmeticExpr^
// ArithmeticExpr: Term ([+|-] Term)*
// Term: Factor ([*|/] Factor)*
// Factor: (ArithmeticExpr) | Value
// Value: DiceGroup | Int | Id
internal static class Parsers
{
    #region Helper Parsers
    public static Parser<int> Integer { get; } =
        from leading in Parse.WhiteSpace.Many()
        from n in Parse.Digit.AtLeastOnce()
        select int.Parse(string.Join("", n));

    public static Parser<T> Parens<T>(Parser<T> inner)
    {
        var result =
            from leading in Parse.WhiteSpace.Many()
            from leftParen in Parse.Char('(')
            from elem in inner
            from white in Parse.WhiteSpace.Many()
            from rightParen in Parse.Char(')')
            select elem;
        return result;
    }
    #endregion

    #region Value Parsers
    // DiceGroup ::= [Int]d[Int]
    public static Parser<DiceGroup> DiceGroup { get; } =
        from leading in Parse.WhiteSpace.Many()
        from count in Integer
        from d in Parse.Char('d')
        from sides in Integer
        select new DiceGroup(count, sides);

    public static Parser<DiceGroupExpr> DiceGroupExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from value in DiceGroup
        select new DiceGroupExpr(value);

    // Int ::= [Digit]+
    public static Parser<IntExpr> IntExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from value in Integer
        select new IntExpr(value);

    // Id ::= [a-Z]+
    public static Parser<IdentifierExpr> IdentifierExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from value in Parse.Letter.AtLeastOnce()
        select new IdentifierExpr(string.Join("", value));

    // Value ::= DiceGroup | Int | Id
    public static Parser<IRollableExpr> Value { get; } = (DiceGroupExpr as Parser<IRollableExpr>).Or(IntExpr).Or(IdentifierExpr);
    #endregion

    #region Binary Operators
    public static Parser<BinaryOperator> Op(char ch, BinaryOperator binOp) =>
        from leading in Parse.WhiteSpace.Many()
        from opChar in Parse.Char(ch)
        select binOp;

    public static Parser<BinaryOperator> AddOp { get; } = Op('+', (a, b) => new AddExpr(a, b));
    public static Parser<BinaryOperator> SubOp { get; } = Op('-', (a, b) => new SubExpr(a, b));
    public static Parser<BinaryOperator> MulOp { get; } = Op('*', (a, b) => new MulExpr(a, b));
    public static Parser<BinaryOperator> DivOp { get; } = Op('/', (a, b) => new DivExpr(a, b));
    #endregion

    // ArithmeticExpr ::= Term ([+|-] Term)*
    public static Parser<IRollableExpr> ArithmeticExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from term in Term
        from rightTerms in RightArithmetic.Many()
        select RightCurriedOperator.Chain(rightTerms)?.Invoke(term) ?? term;

    // [+|-] Term
    private static Parser<RightCurriedOperator> RightArithmetic { get; } =
        from leading in Parse.WhiteSpace.Many()
        from op in AddOp.Or(SubOp)
        from term in Term
        select new RightCurriedOperator(op, term);

    // Factor ::= (ArithmeticExpr) | Value
    public static Parser<IRollableExpr> Factor { get; } = Parens(ArithmeticExpr).Or(Value);

    // Term ::= Factor ([*|/] Factor)*
    public static Parser<IRollableExpr> Term { get; } =
        from leading in Parse.WhiteSpace.Many()
        from factor in Factor
        from rightExprs in RightTerm.Many()
        select RightCurriedOperator.Chain(rightExprs)?.Invoke(factor) ?? factor;

    // [*|/] Term
    private static Parser<RightCurriedOperator> RightTerm { get; } =
        from leading in Parse.WhiteSpace.Many()
        from op in MulOp.Or(DivOp)
        from factor in Factor
        select new RightCurriedOperator(op, factor);

    // Expr ::= $ArithmeticExpr^
    public static Parser<IRollableExpr> Expr { get; } =
        (from leading in Parse.WhiteSpace.Many()
         from expr in ArithmeticExpr
         from trailing in Parse.WhiteSpace.Many()
         select expr).End();
}

// Helper class that holds an operator and the right operand. For example: ? + 3.
// Useful when parsing the `Term` and `ArithmeticExpr` portions of the grammar.
internal sealed class RightCurriedOperator
{
    private readonly BinaryOperator _curried;
    private readonly IRollableExpr _rightSide;
    public RightCurriedOperator(BinaryOperator binOperator, IRollableExpr rightSide) =>
        (_curried, _rightSide) = (binOperator, rightSide);
    public BinopExpr Invoke(IRollableExpr a) => _curried.Invoke(a, _rightSide);
    public RightCurriedOperator Chain(RightCurriedOperator toChain) =>
        new(_curried, toChain.Invoke(_rightSide));
    public static RightCurriedOperator? Chain(IEnumerable<RightCurriedOperator> ops) =>
        ops.Count() == 0 ? null : ops.Aggregate((leftSide, acc) => leftSide.Chain(acc));
}

// A function which takes two expressions and combines them into a BinopExpr
internal delegate BinopExpr BinaryOperator(IRollableExpr a, IRollableExpr b);