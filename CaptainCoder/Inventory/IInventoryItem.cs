namespace CaptainCoder.Inventory;

/// <summary>
/// The <see cref="IInventoryItem"/> interface specifies an item that can be
/// placed within an <see cref="IInventoryGrid{T}"/>. 
/// </summary>
public interface IInventoryItem
{
    /// <summary>
    /// The amount of space this <see cref="IInventoryItem"/> occupies.
    /// </summary>
    public Dimensions Size => new (1, 1);
}
