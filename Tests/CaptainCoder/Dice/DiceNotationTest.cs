using Moq;
using CaptainCoder.Core;
namespace CaptainCoder.Dice;

public class DiceNotationTest
{
    [Fact]
    public void TestParseAdd()
    {
        Mock<IRandom> randomSource = new ();
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
        Mock<IRandom> randomSource = new ();
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
        Mock<IRandom> randomSource = new ();
        Mock<IRollContext> context = new();
        DiceNotation threeDeeSix = DiceNotation.Parse("3d6", randomSource.Object);

        randomSource.SetupSequence(r => r.Next(1, 7)).Returns(1).Returns(3).Returns(5);
        RollResult result = threeDeeSix.Roll(context.Object);
        Assert.Equal(9, result.Value);

        randomSource.SetupSequence(r => r.Next(1, 7)).Returns(1).Returns(1).Returns(1);

        Assert.Equal(3, threeDeeSix.Roll(context.Object).Value);
    }
}