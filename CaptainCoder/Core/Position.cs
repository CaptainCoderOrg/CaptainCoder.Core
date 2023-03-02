namespace CaptainCoder.Core;
public readonly record struct Position(int Row, int Col)
{
    public static implicit operator Position((int row, int col) pair) => new Position(pair.row, pair.col);
}