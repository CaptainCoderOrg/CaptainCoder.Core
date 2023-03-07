using CaptainCoder.Core;
using System.Collections.Generic;
namespace CaptainCoder.Inventory;

/// <summary>
/// The <see cref="IInventoryGrid"/> defines a grid of cells that can hold <see cref="IInventoryItem"/>s.
/// </summary>
public interface IInventoryGrid
{
    /// <summary>
    /// The <see cref="Size"/> of this <see cref="IInventoryGrid"/>
    /// </summary>
    public Size GridSize => new (4,10);

    /// <summary>
    /// A enumerable containing one entry for every <see cref="IInventoryItem"/> in this
    /// <see cref="IInventoryGrid"/>
    /// </summary>
    public IEnumerable<GridSlot> Items { get; }

    /// <summary>
    /// Attempts to retrieve an item at the specified position. If an item
    /// exists in that position, returns true and populates <paramref
    /// name="item"/> otherwise returns false and the value of <parameref
    /// name="item"/> is undefined.
    /// </summary>
    public bool TryGetItemAt(Position position, out IInventoryItem item);

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
    public bool TrySetItemAt(Position topLeft, IInventoryItem item);

    /// <summary>
    /// Attempts to add the specified <paramref name="item"/> to this inventory
    /// by placing its top left corner in the specified <see cref="Position"/>.
    /// Returns true if the <paramref name="item"/> was added successfully and
    /// false otherwise. The <paramref name="removed"/> will be set to any item
    /// that was removed from the inventory by adding the specified item.
    /// </summary>
    public bool TrySetItemAt(Position topLeft, IInventoryItem toAdd, out IInventoryItem? removed);

    /// <summary>
    /// Attempts to remove an item at the specified position. Returns true if an
    /// item was present and sets <paramref name="item"/> to the removed item.
    /// Otherwise returns false and <paramref name="item"/> is undefined.
    /// </summary>
    public bool TryRemoveItemAt(Position position, out IInventoryItem item);

    /// <summary>
    /// A <see cref="GridSlot"/> represents where the top left corner
    /// of an <see cref="IInventoryItem"/> is within a <see cref="IInventoryGrid"/>
    /// </summary>
    /// <returns></returns>
    public record GridSlot(Position TopLeft, IInventoryItem Item, IInventoryGrid Grid);
}