using System;
using System.Collections.Generic;
using System.Linq;
namespace CaptainCoder.Core;

[Serializable]
public readonly record struct Position(int Row, int Col)
{
    public static implicit operator Position((int row, int col) pair) => new Position(pair.row, pair.col);
    public static implicit operator Position(MutablePosition mutablePosition) => mutablePosition.Freeze();
}