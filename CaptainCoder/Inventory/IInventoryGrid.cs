namespace CaptainCoder.Inventory;

/// <summary>
/// The <see cref="IInventoryGrid{T}"/> defines a grid of cells that can hold <see cref="IInventoryItem"/>s.
/// </summary>
public interface IInventoryGrid<T> where T : IInventoryItem
{
    /// <summary>
    /// The <see cref="Dimensions"/> of this <see cref="IInventoryGrid{T}"/>
    /// </summary>
    public Dimensions GridSize => new (4,10);

    /// <summary>
    /// A enumerable containing one entry for every <see cref="IInventoryItem"/> in this
    /// <see cref="IInventoryGrid{T}"/>
    /// </summary>
    public IEnumerable<GridSlot> Items { get; }

    /// <summary>
    /// Attempts to retrieve an item at the specified position. If an item
    /// exists in that position, returns true and populates <paramref
    /// name="item"/> otherwise returns false and the value of <paramref
    /// name="item"/> is undefined.
    /// </summary>
    public bool TryGetItemAt(Position position, out T? item);

    /// <summary>
    /// Returns true if the specified <see cref="Position"/> is occupied and
    /// false otherwise.
    /// </summary>
    public bool IsOccupied(Position position);

    /// <summary>
    /// Attempts to add the specified <paramref name="item"/> to this inventory
    /// by placing its top left corner in the specified <see cref="Position"/>.
    /// Returns true if the <paramref name="item"/> was added successfully and
    /// false otherwise. This method will fail if an item occupies any of the
    /// spaces this item requires.
    /// </summary>
    public bool TrySetItemAt(Position topLeft, T item);

    /// <summary>
    /// Attempts to add the specified <paramref name="item"/> to this inventory
    /// by placing its top left corner in the specified <see cref="Position"/>.
    /// Returns true if the <paramref name="item"/> was added successfully and
    /// false otherwise. For convenience, on success if exactly one item
    /// occupied the area that <paramref name="item"/> now occupies, the
    /// <paramref name="removedItem"/> is set to that item. If no such item
    /// existed, the value will be null.
    /// </summary>
    public bool TrySetItemAt(Position topLeft, T item, out T? removedItem);

    /// <summary>
    /// Attempts to remove an item at the specified position. Returns true if an
    /// item was present and sets <paramref name="item"/> to the removed item.
    /// Otherwise returns false and <paramref name="item"/> is undefined.
    /// </summary>
    public bool TryRemoveItemAt(Position position, out T? item);

    /// <summary>
    /// A <see cref="GridSlot"/> represents where the top left corner
    /// of an <see cref="IInventoryItem"/> is within a <see cref="IInventoryGrid{T}"/>
    /// </summary>
    public record GridSlot(Position TopLeft, T Item, IInventoryGrid<T> Grid);
}