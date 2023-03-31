using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
namespace CaptainCoder.Dice;
internal static class Parsers
{

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

    public static Parser<IdentifierExpr> IdentifierExpr  { get; } =
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

    public static Parser<LeftOp> DivMul { get; } =
        from leading in Parse.WhiteSpace.Many()
        from leftExpr in Value
        from op in MulPartial.Or(DivPartial)
        select new LeftOp(leftExpr, op);

    public static Parser<LeftOp> AddSub { get; } =
        from leading in Parse.WhiteSpace.Many()
        from leftExpr in Value
        from op in AddPartial.Or(SubPartial)
        select new LeftOp(leftExpr, op);

    public static Parser<LeftOp> BinOp { get; } =
        from leading in Parse.WhiteSpace.Many()
        from leftExpr in DivMul.Or(AddSub)
        select leftExpr;

    public static Parser<LeftOp> BinOps { get; } =
        from leading in Parse.WhiteSpace.Many()
        from leftOps in BinOp.AtLeastOnce()
        select LeftOp.Chain(leftOps);

    public static Parser<BinopExpr> BinopExpr { get; } =
        from leading in Parse.WhiteSpace.Many()
        from ops in BinOps
        from lastValue in Value
        select ops.Invoke(lastValue);

    public static Parser<IRollableExpr> Expr { get; } =
        OptionalParens(BinopExpr).Or(Value);

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

    public static Parser<T> OptionalParens<T>(Parser<T> inner)
    {
        var result =
            from leading in Parse.WhiteSpace.Many()
            from leftParen in Parse.Char('(')
            from elem in inner
            from white in Parse.WhiteSpace.Many()
            from rightParen in Parse.Char(')')
            select elem;
        return result.Or(inner);
    }
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