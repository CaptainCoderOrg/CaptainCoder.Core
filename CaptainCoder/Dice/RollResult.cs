public record RollResult(string Message, int Value)
{
    public static RollResult operator +(RollResult a, RollResult b)
    {
        int value = a.Value + b.Value;
        string message = $"{a.Message} + {b.Message} = {value}";
        return new RollResult(message, value);
    }

    public static RollResult operator -(RollResult a, RollResult b)
    {
        int value = a.Value - b.Value;
        string message = $"{a.Message} - {b.Message} = {value}";
        return new RollResult(message, value);
    }

    public static RollResult operator *(RollResult a, RollResult b)
    {
        int value = a.Value * b.Value;
        string message = $"{a.Message} * {b.Message} = {value}";
        return new RollResult(message, value);
    }

    public static RollResult operator /(RollResult a, RollResult b)
    {
        int denom = b.Value == 0 ? 1 : b.Value;
        int value = a.Value / denom;
        string message = $"{a.Message} / {b.Message} = {value}";
        return new RollResult(message, value);
    }

    public static explicit operator int(RollResult result) => result.Value; 
}