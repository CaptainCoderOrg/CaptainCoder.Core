namespace CaptainCoder.Inventory;

/// <summary>
/// The <see cref="IHasInventoryGrid{T}"/> interface is used to identify 
/// data types that are composed with an <see cref="IInventoryGrid{T}"/>.
/// </summary>
public interface IHasInventoryGrid<T> where T : class, IInventoryItem
{
    /// <summary>
    /// Retrieves the <see cref="IInventoryGrid{T}"/>
    /// </summary>
    public IInventoryGrid<T> InventoryGrid { get; }
}