using System;
namespace CaptainCoder.Core;

/// <summary>
/// A discrete mutable position defined by a row and column. For convenience,
/// this struct can be implicitly cast to a <see cref="Position"/>.
/// </summary>
[Serializable]
public struct MutablePosition
{
    /// <summary></summary>
    public int Row;
    /// <summary></summary>
    public int Col;
    /// <summary>
    /// Freezes this struct into an immutable <see cref="Position"/>
    /// </summary>
    public Position Freeze() => new (Row, Col);
}