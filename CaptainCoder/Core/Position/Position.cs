using System;
namespace CaptainCoder.Core;

/// <summary>
/// A discrete readonly position defined by a row and column. For convenience,
/// this struct supports an implicit cast from (int, int) tuples and the <see
/// cref="MutablePosition"/> struct.
/// </summary>
[Serializable]
public readonly record struct Position(int Row, int Col)
{
    /// <summary>
    /// Allows (int, int) tuples to be used anywhere a Position can be used. Be careful not to 
    /// do this when using a position as a key in a HashSet or Dictionary.
    /// </summary>
    public static implicit operator Position((int row, int col) pair) => new(pair.row, pair.col);

    /// <summary>
    /// Allows MutablePosition to be used anywhere a Position could be used.
    /// </summary>
    public static implicit operator Position(MutablePosition mutablePosition) => mutablePosition.Freeze();

    /// <summary>
    /// Sums the row and column values together
    /// </summary>
    public static Position operator +(Position a, Position b) => new(a.Row + b.Row, a.Col + b.Col);

    /// <summary>
    /// Calculates the simple difference in row and column values
    /// </summary>
    public static Position operator -(Position a, Position b) => new(a.Row - b.Row, a.Col - b.Col);
}