using System;
using System.Collections.Generic;
using CaptainCoder.Core;

namespace CaptainCoder.Dice;

internal interface IRollableExpr
{
    public RollResult Eval(IRollContext context, IRandom rng);
    public string Standardized { get; }
}

internal record IntExpr(int Value) : IRollableExpr
{
    public RollResult Eval(IRollContext context, IRandom rng) => new(Value.ToString(), Value);
    public string Standardized => Value.ToString();
}

internal record DiceGroupExpr(DiceGroup Group) : IRollableExpr
{
    public RollResult Eval(IRollContext context, IRandom rng) => Group.Roll(rng);
    public string Standardized => Group.Notation;
}

internal record IdentifierExpr(string Id) : IRollableExpr
{
    public RollResult Eval(IRollContext context, IRandom rng) => new($"{context.Lookup(Id)} ({Id})", context.Lookup(Id));
    public string Standardized => Id;
}

internal record BinopExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand, Func<RollResult, RollResult, RollResult> Operator, char Op) : IRollableExpr
{
    public RollResult Eval(IRollContext context, IRandom rng) => Operator(LeftOperand.Eval(context, rng), RightOperand.Eval(context, rng));
    public string Standardized => $"({LeftOperand.Standardized} {Op} {RightOperand.Standardized})";
}

internal record AddExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand) : BinopExpr(LeftOperand, RightOperand, (a, b) => a + b, '+');
internal record SubExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand) : BinopExpr(LeftOperand, RightOperand, (a, b) => a - b, '-');
internal record MulExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand) : BinopExpr(LeftOperand, RightOperand, (a, b) => a * b, '*');
internal record DivExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand) : BinopExpr(LeftOperand, RightOperand, (a, b) => a / b, '/');