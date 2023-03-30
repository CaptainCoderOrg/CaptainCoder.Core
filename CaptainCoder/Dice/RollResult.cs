namespace CaptainCoder.Dice;
/// <summary>
/// A <see cref="RollResult"/> provides a message describing the result of the roll as well as the final value.
/// For convenience, RollResult may be used as an integer value.
/// </summary>
public record RollResult(string Message, int Value)
{
    /// <summary>
    /// Sums two results
    /// </summary>
    public static RollResult operator +(RollResult a, RollResult b)
    {
        int value = a.Value + b.Value;
        string message = $"{a.Message} + {b.Message} = {value}";
        return new RollResult(message, value);
    }

    /// <summary>
    /// Takes the difference of two results
    /// </summary>
    public static RollResult operator -(RollResult a, RollResult b)
    {
        int value = a.Value - b.Value;
        string message = $"{a.Message} - {b.Message} = {value}";
        return new RollResult(message, value);
    }

    /// <summary>
    /// Calculates the product of two results
    /// </summary>
    public static RollResult operator *(RollResult a, RollResult b)
    {
        int value = a.Value * b.Value;
        string message = $"{a.Message} * {b.Message} = {value}";
        return new RollResult(message, value);
    }

    /// <summary>
    /// Calculates the quotient of two results
    /// </summary>
    public static RollResult operator /(RollResult a, RollResult b)
    {
        int denom = b.Value == 0 ? 1 : b.Value;
        int value = a.Value / denom;
        string message = $"{a.Message} / {b.Message} = {value}";
        return new RollResult(message, value);
    }

    /// <summary>
    /// For convenience, a RollResult may be used as an integer
    /// </summary>
    public static implicit operator int(RollResult result) => result.Value; 
}