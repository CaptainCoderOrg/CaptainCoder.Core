using System;
using System.Collections.Generic;
namespace CaptainCoder.Core.Dice;

internal class DiceNotationParser
{
    
    private readonly Queue<string> toParse;
    private bool _result;
    private string _errorMessage;
    private IRollableExpr? _expr;

    public DiceNotationParser(string notation)
    {
        toParse = new Queue<string>(notation.Split(new char[0]));
        Init();
    }

    public bool Parse(out IRollableExpr expr, out string errorMessage)
    {
        expr = _expr!;
        errorMessage = _errorMessage;
        return _result;
    }

    private void Init()
    {
        try
        {
            _expr = Parse();
            _errorMessage = string.Empty;
            _result = true;
        }
        catch (FormatException exception)
        {
            _expr = null;
            _errorMessage = exception.Message;
            _result = false;
        }
    }

    private IRollableExpr Parse()
    {

        if (toParse.Count == 1)
        {
            return ParseValue(toParse.Dequeue());
        }
        if (toParse.Count < 3) 
        {     
            throw new FormatException($"Incomplete operator while parsing dice notation: {string.Join("", toParse)}"); 
        }

        IRollableExpr leftValue = ParseValue(toParse.Dequeue());
        Func<IRollableExpr, IRollableExpr, IRollableExpr> opBuilder = ParseBinaryOp(toParse.Dequeue());
        IRollableExpr rightValue = Parse();
        return opBuilder(leftValue, rightValue);
    }
    
    private Func<IRollableExpr, IRollableExpr, IRollableExpr> ParseBinaryOp(string symbol)
    {
        return symbol switch
        {
            "+" => (left, right) => new AddExpr(left, right),
            "-" => (left, right) => new SubExpr(left, right),
            _ => throw new FormatException($"Unsupported operator {symbol}.")
        };
    }

    private IRollableExpr ParseValue(string symbol)
    {
        if (int.TryParse(symbol, out int value)) { return new IntExpr(value); }
        if (DiceGroup.TryParse(symbol, out DiceGroup group)) { return new DiceGroupExpr(group); }

        throw new FormatException($"Expected value but found {symbol}");
    }

    private void HandleChar(char ch)
    {
        if (char.IsWhiteSpace(ch)) { return; }

    }

    private static readonly Dictionary<string, IRollableExpr> _cache = new ();
    public static IRollableExpr Parse(string notation)
    {
        if (_cache.TryGetValue(notation, out var cached))
        {
            return cached;
        }
        DiceNotationParser parser = new (notation);
        if(parser.Parse(out IRollableExpr result, out string errorMessage))
        {
            return result;
        }
        throw new FormatException($"Invalid Dice Notation \"{notation}\": {errorMessage}.");
    }
}

internal interface IRollableExpr
{
    public RollResult Eval(IRollContext context);
    public string Standardized { get; }
}

internal record IntExpr(int Value) : IRollableExpr
{
    public RollResult Eval(IRollContext context) => new (Value.ToString(), Value);
    public string Standardized => Value.ToString();
}

internal record DiceGroupExpr(DiceGroup Group) : IRollableExpr
{
    public RollResult Eval(IRollContext context) => Group.Roll();
    public string Standardized => Group.Standardized;
}

internal record IdentifierExpr(string Id) : IRollableExpr
{
    public RollResult Eval(IRollContext context) => new ($"{context.Lookup(Id)} ({Id})", context.Lookup(Id));
    public string Standardized => Id;
}

internal record BinopExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand, Func<RollResult, RollResult, RollResult> Operator, char Op) : IRollableExpr
{
    public RollResult Eval(IRollContext context) => Operator(LeftOperand.Eval(context), RightOperand.Eval(context));
    public string Standardized => $"({LeftOperand.Standardized} {Op} {RightOperand.Standardized})";
}

internal record AddExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand) : BinopExpr(LeftOperand, RightOperand, (a, b) => a + b, '+');
internal record SubExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand) : BinopExpr(LeftOperand, RightOperand, (a, b) => a - b, '-');
internal record MulExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand) : BinopExpr(LeftOperand, RightOperand, (a, b) => a * b, '*');
internal record DivExpr(IRollableExpr LeftOperand, IRollableExpr RightOperand) : BinopExpr(LeftOperand, RightOperand, (a, b) => a / b, '/');


