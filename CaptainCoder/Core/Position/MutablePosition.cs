using System;
namespace CaptainCoder.Core;

/// <summary>
/// A discrete mutable position defined by a row and column. For convenience,
/// this struct can be implicitly cast to a <see cref="Position"/>.
/// </summary>
[Serializable]
public struct MutablePosition
{
    public int Row;
    public int Col;
    public Position Freeze() => new (Row, Col);
}