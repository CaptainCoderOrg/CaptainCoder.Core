using Moq;
using Sprache;
using CaptainCoder.Core;
namespace CaptainCoder.Dice;

public class DiceNotationTest
{
    [Fact]
    public void TestParseAdd()
    {
        Mock<IRandom> randomSource = new();
        Mock<IRollContext> context = new();
        DiceNotation threeDeeSix = DiceNotation.Parse("3d6 + Strength", randomSource.Object);

        randomSource.SetupSequence(r => r.Next(1, 7)).Returns(1).Returns(3).Returns(5);
        context.Setup(c => c.Lookup("Strength")).Returns(2);

        RollResult result = threeDeeSix.Roll(context.Object);
        Assert.Equal(11, result.Value);

        randomSource.SetupSequence(r => r.Next(1, 7)).Returns(1).Returns(1).Returns(1);
        randomSource.Setup(r => r.Next(1, 7)).Returns(1);
        randomSource.Setup(r => r.Next(1, 7)).Returns(1);
        context.Setup(c => c.Lookup("Strength")).Returns(5);

        Assert.Equal(8, threeDeeSix.Roll(context.Object).Value);
    }

    [Fact]
    public void TestParseSub()
    {
        Mock<IRandom> randomSource = new();
        Mock<IRollContext> context = new();
        DiceNotation threeDeeSix = DiceNotation.Parse("3d6 - Strength", randomSource.Object);

        randomSource.SetupSequence(r => r.Next(1, 7)).Returns(1).Returns(3).Returns(5);
        context.Setup(c => c.Lookup("Strength")).Returns(2);

        RollResult result = threeDeeSix.Roll(context.Object);
        Assert.Equal(7, result.Value);

        randomSource.SetupSequence(r => r.Next(1, 7)).Returns(1).Returns(1).Returns(1);
        context.Setup(c => c.Lookup("Strength")).Returns(5);

        Assert.Equal(-2, threeDeeSix.Roll(context.Object).Value);
    }

    [Fact]
    public void TestSimple()
    {
        Mock<IRandom> randomSource = new();
        Mock<IRollContext> context = new();
        DiceNotation threeDeeSix = DiceNotation.Parse("3d6");

        randomSource.SetupSequence(r => r.Next(1, 7)).Returns(1).Returns(3).Returns(5);
        RollResult result = threeDeeSix.Roll(null, randomSource.Object);
        Assert.Equal(9, result.Value);

        randomSource.SetupSequence(r => r.Next(1, 7)).Returns(1).Returns(1).Returns(1);

        Assert.Equal(3, threeDeeSix.Roll(context.Object, randomSource.Object).Value);
    }

    [Fact]
    public void TestParensComplex()
    {
        Mock<IRandom> randomSource = new();
        Mock<IRollContext> context = new();
        DiceNotation complex = DiceNotation.Parse("(HP/1d4)+(1d4 * 2)");
        context.Setup(c => c.Lookup("HP")).Returns(8);
        randomSource.SetupSequence(r => r.Next(1, 5)).Returns(2).Returns(3);
        // (8/2) + (3*2)
        // 4 + 6
        // 10
        RollResult result = complex.Roll(context.Object, randomSource.Object);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void TestFactorParser()
    {
        IResult<IRollableExpr> result = Parsers.SingleTerm.TryParse("5");
        RollResult rollResult = result.Value.Eval(null, null);
        Assert.Equal(5, rollResult.Value);

        result = Parsers.SingleTerm.TryParse("5 * 2");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(10, rollResult.Value);

        result = Parsers.SingleTerm.TryParse("5 * 8 / 2");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(20, rollResult.Value);
    }

    [Fact]
    public void TestArithmeticParser()
    {
        IResult<IRollableExpr> result = Parsers.SingleArithmetic.TryParse("5");
        RollResult rollResult = result.Value.Eval(null, null);
        Assert.Equal(5, rollResult.Value);

        result = Parsers.SingleArithmetic.TryParse("5 * 2");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(10, rollResult.Value);

        result = Parsers.SingleArithmetic.TryParse("5 * 8 / 2");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(20, rollResult.Value);

        result = Parsers.SingleArithmetic.TryParse("5 * 2 + 7");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(17, rollResult.Value);

        result = Parsers.SingleArithmetic.TryParse("10 / 2");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(5, rollResult.Value);

        result = Parsers.SingleArithmetic.TryParse("2 + 10 / 2");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(7, rollResult.Value);

        result = Parsers.SingleArithmetic.TryParse("10 * 2 + 10 / 2");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(25, rollResult.Value);
    }

    [Fact]
    public void TestParens()
    {
        IResult<IRollableExpr> result = Parsers.SingleArithmetic.TryParse("(2 + 2)");
        RollResult rollResult = result.Value.Eval(null, null);
        Assert.Equal(4, rollResult.Value);

        result = Parsers.SingleArithmetic.TryParse("(2 + 2) * (7 - 2)");
        rollResult = result.Value.Eval(null, null);
        Assert.Equal(20, rollResult.Value);
    }

    [Fact]
    public void TestPrecedent()
    {
        DiceNotation precedent = DiceNotation.Parse("1 + 2 * 3 - 4 / 2");
        RollResult result = precedent.Roll(null);
        Assert.Equal(5, result.Value);
    }
}