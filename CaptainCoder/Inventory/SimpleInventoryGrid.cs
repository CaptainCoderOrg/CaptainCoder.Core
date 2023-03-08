using System.Diagnostics;

namespace CaptainCoder.Inventory;

/// <summary>
/// 
/// </summary>
public class SimpleInventoryGrid<T> : IInventoryGrid<T> where T : class, IInventoryItem
{

    private readonly Dictionary<T, Position> _itemLookup = new();
    private readonly T?[,] _grid;

    /// <summary>
    /// Initializes an instance of <see cref="SimpleInventoryGrid{T}"/> with a
    /// GridSize of 4 Rows and 10 Columns.
    /// </summary>
    public SimpleInventoryGrid()
    {
        _grid = new T[GridSize.Rows, GridSize.Columns];
    }

    /// <summary>
    /// Initializes an instance of <see cref="SimpleInventoryGrid{T}"/> with the
    /// specified <paramref name="gridSize"/>
    /// </summary>
    public SimpleInventoryGrid(Dimensions gridSize) : this()
    {
        GridSize = gridSize;
    }

    /// <inheritdoc/>
    public Dimensions GridSize { get; init; } = new(4, 10);

    /// <inheritdoc/>
    public IEnumerable<IInventoryGrid<T>.GridSlot> Items
    {
        get
        {
            foreach ((T item, Position topLeft) in _itemLookup)
            {
                yield return new IInventoryGrid<T>.GridSlot(topLeft, item, this);
            }
        }
    }

    /// <inheritdoc/>
    public bool IsOccupied(Position position) => _grid[position.Row, position.Col] != null;

    private bool IsInBounds(Position position) => 
            !(position.Row < 0 ||
              position.Col < 0 ||
              position.Row >= GridSize.Rows ||
              position.Col >= GridSize.Columns);

    /// <inheritdoc/>
    public bool TryGetItemAt(Position position, out T? item)
    {
        item = null;
        if (!IsInBounds(position) || !IsOccupied(position)) { return false; }
        item = _grid[position.Row, position.Col];
        return true;
    }

    /// <inheritdoc/>
    public bool TryRemoveItemAt(Position position, out T? item)
    {
        item = null;
        if (!IsOccupied(position)) { return false; }
        item = _grid[position.Row, position.Col]!;
        Position topLeft = _itemLookup[item];
        _itemLookup.Remove(item);
        foreach (Position itemCell in item.Size)
        {
            Position inGrid = topLeft + itemCell;
            _grid[inGrid.Row, inGrid.Col] = null;
        }
        return true;
    }

    /// <inheritdoc/>
    public bool TrySetItemAt(Position topLeft, T item)
    {
        bool canSet = item.Size.Positions.Any(p => !IsInBounds(p + topLeft) || IsOccupied(p + topLeft));
        if (canSet) { return false; }

        _itemLookup[item] = topLeft;
        foreach (Position itemCell in item.Size)
        {
            Position inGrid = topLeft + itemCell;
            _grid[inGrid.Row, inGrid.Col] = item;
        }
        return true;
    }

    /// <inheritdoc/>
    public bool TrySetItemAt(Position topLeft, T item, out T? removedItem)
    {
        removedItem = null;
        // No items occupy this space
        if (TrySetItemAt(topLeft, item)) { return true; }

        // If more than one item occupies the space, return false
        if (!IntersectsWithOne(topLeft, item, out T itemToRemove)) { return false; }

        // Exactly one item occupies the space
        Position toRemoveTopLeft = _itemLookup[itemToRemove];
        bool wasRemoved = TryRemoveItemAt(toRemoveTopLeft, out removedItem);
        Debug.Assert(wasRemoved, "Item could not be removed, this should not be possible.");
        bool wasAdded = TrySetItemAt(topLeft, item);
        Debug.Assert(wasAdded, "Item could not be added, this should not be possible");
        return true;
    }

    private bool IntersectsWithOne(Position topLeft, T item, out T foundItem)
    {
        HashSet<T> found = new();
        foundItem = null!;
        foreach (Position itemCell in item.Size)
        {
            Position inGrid = topLeft + itemCell;
            if (!IsOccupied(inGrid)) { continue; }
            foundItem = _grid[inGrid.Row, inGrid.Col]!;
            found.Add(foundItem);
            if (found.Count > 1) { return false; }
        }
        Debug.Assert(foundItem != null, "Impossible state detected. Should not call unless it is known that at least one item exists.");
        return true;
    }
}