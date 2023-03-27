namespace CaptainCoder.Core;

/// <summary>
/// A simple interface to wrap random number generators.
/// </summary>
public interface IRandom
{
    private static IRandom? s_Shared;
    /// <summary>
    /// Returns a shared instance for generating numbers.
    /// </summary>
    public static IRandom Shared => s_Shared ??= new RandomWrapper();

    /// <summary>
    /// Instantiates a new default instance.
    /// </summary>
    public static IRandom Instantiate() => new RandomWrapper();

    /// <summary>
    /// Instantiates a new default instance specifying the initial seed.
    /// </summary>
    public static IRandom Instantiate(int seed) => new RandomWrapper(seed);


    /// <summary>
    /// Returns a non-negative random integer.
    /// </summary>
    public int Next();

    /// <summary>
    /// Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    public int Next(int maxValue);

    /// <summary>
    /// Returns a random integer that is within a specified range.
    /// </summary>
    public int Next(int minValue, int maxValue);

    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to 0.0,
    /// and less than 1.0.
    /// </summary>
    public double NextDouble();
}

internal sealed class RandomWrapper : IRandom
{
    private readonly System.Random _rng;
    internal RandomWrapper(int seed) => _rng = new System.Random(seed);
    internal RandomWrapper() => _rng = new System.Random();
    public int Next() => _rng.Next();
    public int Next(int maxValue) => _rng.Next(maxValue);
    public int Next(int minValue, int maxValue) => _rng.Next(minValue, maxValue);
    public double NextDouble() => _rng.NextDouble();
}