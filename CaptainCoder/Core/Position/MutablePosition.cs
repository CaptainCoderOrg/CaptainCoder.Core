using System;
namespace CaptainCoder.Core;
[Serializable]
public struct MutablePosition
{
    public int Row;
    public int Col;
    public Position Freeze() => new Position(Row, Col);
}