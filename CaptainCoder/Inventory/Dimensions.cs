namespace CaptainCoder.Inventory;

/// <summary>
/// The readonly <see cref="Dimensions"/> struct defines a discrete rectangular
/// shape using rows and columns.
/// </summary>
[Serializable]
public readonly record struct Dimensions : IEnumerable<Position>
{
    private readonly int _rows = 1;
    private readonly int _columns = 1;

    /// <summary>
    /// Constructs an instance of <see cref="Dimensions"/> specifying the number
    /// of rows and columns it contains.
    /// </summary>
    public Dimensions(int rows, int columns) => (Rows, Columns) = (rows, columns);

    /// <summary>
    /// Retrieves each position represented within this dimension. The returned
    /// order will be left to right and top to bottom.
    /// </summary>
    /// <example>
    /// <code>
    /// Dimension d = new Dimension(3,3);
    /// Console.WriteLine(string.Joint(", ", d.Positions));
    /// // $ (0,0), (0, 1), (0, 2), (1, 0), (1, 1), (1, 2), (2, 0), (2, 1), (2, 2)
    /// </code>
    /// </example>
    public IEnumerable<Position> Positions
    {
        get
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    yield return new Position(r, c);
                }
            }
        }
    }

    /// <value>Must be initialized to a positive value.</value>
    public readonly int Rows
    {
        get => _rows;
        init
        {
            if (value < 1) { throw new ArgumentException("Dimensions must be positive."); }
            _rows = value;
        }
    }

    /// <value>Must be initialized to a positive value.</value>
    public readonly int Columns
    {
        get => _columns;
        init
        {
            if (value < 1) { throw new ArgumentException("Dimensions must be positive."); }
            _columns = value;
        }
    }

    /// <summary>
    /// Provides an enumerator that iterates over <see cref="Dimensions.Positions"/>.
    /// </summary>
    public IEnumerator<Position> GetEnumerator() => Positions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}