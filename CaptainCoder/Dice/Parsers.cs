using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
namespace CaptainCoder.Dice;
internal static class Parsers
{
    /*
    Grammar:
    Expr: ArithmeticExpr
    ArithmeticExpr: Term ((+|-) Term)*
    Term: Factor ((*|/) Factor)*
    Factor: (ArithmeticExpr) | Value
    Value: diceGroup | int | id

    */

    public static Parser<DiceGroup> DiceGroup { get; } =
        from leading in Parse.WhiteSpace.Many()
        from count in Integer
        from d in Parse.Char('d')
        from sides in Integer
        select new DiceGroup(count, sides);

    private static Parser<int> Integer { get; } =
        from leading in Parse.WhiteSpace.Many()
        from n in Parse.Digit.AtLeastOnce()
        select int.Parse(string.Join("", n));

    public static Parser<IntExpr> IntExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from value in Integer
        select new IntExpr(value);

    public static Parser<DiceGroupExpr> DiceGroupExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from value in DiceGroup
        select new DiceGroupExpr(value);

    public static Parser<IdentifierExpr> IdentifierExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from value in Parse.Letter.Many()
        select new IdentifierExpr(string.Join("", value));
    public static Parser<IRollableExpr> Value { get; } = (DiceGroupExpr as Parser<IRollableExpr>).Or(IntExpr).Or(IdentifierExpr);

    public static Parser<CurriedBinop> AddPartial { get; } =
        from leading in Parse.WhiteSpace.Many()
        from plus in Parse.Char('+')
        select new CurriedBinop((IRollableExpr a, IRollableExpr b) => new AddExpr(a, b));
    public static Parser<CurriedBinop> SubPartial { get; } =
        from leading in Parse.WhiteSpace.Many()
        from plus in Parse.Char('-')
        select new CurriedBinop((IRollableExpr a, IRollableExpr b) => new SubExpr(a, b));
    public static Parser<CurriedBinop> MulPartial { get; } =
        from leading in Parse.WhiteSpace.Many()
        from plus in Parse.Char('*')
        select new CurriedBinop((IRollableExpr a, IRollableExpr b) => new MulExpr(a, b));
    public static Parser<CurriedBinop> DivPartial { get; } =
        from leading in Parse.WhiteSpace.Many()
        from plus in Parse.Char('/')
        select new CurriedBinop((IRollableExpr a, IRollableExpr b) => new DivExpr(a, b));

    public static Parser<IRollableExpr> SingleArithmetic { get; } =
        from leading in Parse.WhiteSpace.Many()
        from term in SingleTerm
        from rightTerms in RightArithmetic.Many()
        select RightOp.Chain(rightTerms)?.Invoke(term) ?? term;

    public static Parser<RightOp> RightArithmetic { get; } =
        from leading in Parse.WhiteSpace.Many()
        from op in AddPartial.Or(SubPartial)
        from term in SingleTerm
        select new RightOp(op, term);

    public static Parser<IRollableExpr> Factor { get; } = Parens(SingleArithmetic).Or(Value);

    public static Parser<IRollableExpr> SingleTerm { get; } =
        from leading in Parse.WhiteSpace.Many()
        from factor in Factor
        from rightExprs in RightTerm.Many()
        select RightOp.Chain(rightExprs)?.Invoke(factor) ?? factor;

    public static Parser<RightOp> RightTerm { get; } =
        from leading in Parse.WhiteSpace.Many()
        from op in MulPartial.Or(DivPartial)
        from factor in Factor
        select new RightOp(op, factor);

    public static Parser<IRollableExpr> Expr { get; } = SingleArithmetic;

    public static Parser<LeftOp> LeftExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from left in Expr
        from op in MulPartial.Or(DivPartial).Or(AddPartial).Or(SubPartial)
        select new LeftOp(left, op);

    public static Parser<IRollableExpr> Expression { get; } =
        from leading in Parse.WhiteSpace.Many()
        from left in LeftExpr.Many()
        from last in Expr
        from trailing in Parse.WhiteSpace.Many()
        select LeftOp.Chain(left)?.Invoke(last) ?? last;

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
}

internal class RightOp
{
    private CurriedBinop _curried;
    private IRollableExpr _rightSide;
    public RightOp(CurriedBinop curried, IRollableExpr rightSide) => (_curried, _rightSide) = (curried, rightSide);
    public BinopExpr Invoke(IRollableExpr a) => _curried.Invoke(a, _rightSide);
    public RightOp Chain(RightOp toChain) => new(_curried, toChain.Invoke(_rightSide));
    public static RightOp? Chain(IEnumerable<RightOp> ops) => ops.Count() == 0 ? null :
        ops.Aggregate((leftSide, acc) => leftSide.Chain(acc));
}

internal class LeftOp
{
    private CurriedBinop _curried;
    private IRollableExpr _leftSide;
    public LeftOp(IRollableExpr leftSide, CurriedBinop curried) => (_curried, _leftSide) = (curried, leftSide);
    public BinopExpr Invoke(IRollableExpr b) => _curried.Invoke(_leftSide, b);
    public LeftOp Chain(LeftOp toChain) => new LeftOp(Invoke(toChain._leftSide), toChain._curried);
    public static LeftOp? Chain(IEnumerable<LeftOp> ops) => ops.Count() == 0 ? null : ops.Aggregate((left, acc) => left.Chain(acc));
}

internal class CurriedBinop
{
    private Func<IRollableExpr, IRollableExpr, BinopExpr> _curried;
    public CurriedBinop(Func<IRollableExpr, IRollableExpr, BinopExpr> curried) => _curried = curried;
    public BinopExpr Invoke(IRollableExpr a, IRollableExpr b) => _curried.Invoke(a, b);
}