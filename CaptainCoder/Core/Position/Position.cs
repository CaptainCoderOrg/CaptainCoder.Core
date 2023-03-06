using System;
namespace CaptainCoder.Core;

/// <summary>
/// A discrete readonly position defined by a row and column. For convenience,
/// this struct supports an implicit cast from (int, int) tuples and the <see
/// cref="MutablePosition/> struct.
/// </summary>
[Serializable]
public readonly record struct Position(int Row, int Col)
{
    public static implicit operator Position((int row, int col) pair) => new (pair.row, pair.col);
    public static implicit operator Position(MutablePosition mutablePosition) => mutablePosition.Freeze();
}