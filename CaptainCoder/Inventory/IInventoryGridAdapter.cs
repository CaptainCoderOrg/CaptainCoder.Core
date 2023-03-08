namespace CaptainCoder.Inventory;

/// <summary>
/// The <see cref="IInventoryGridAdapter{T}"/> interface provides a default
/// implementation for the <see cref="IInventoryGrid{T}"/> interface through the
/// <see cref="IHasInventoryGrid{T}"/> interface. See the example below for how
/// to utilize it.
/// </summary>
/// <example>
/// In Unity, this is intended to be used with a ScriptableObject that delegates
/// the work to an underlying implementation (<see cref="SimpleInventoryGrid{T}"/>):
/// <code>
/// public class InventoryGridData : ScriptableObject, IInventoryGridAdapter&lt;ItemType&gt; 
/// {
///     [SerializeField]
///     private MutableDimensions _dimensions = new (1,1);
///     private IInventoryGrid&lt;ItemType&gt; _grid;
///     public IInventoryGrid&lt;ItemType&gt; InventoryGrid => 
///                 _grid ??= new SimpleInventoryGrid&lt;ItemType&gt;(_dimensions.Freeze());
/// }
/// </code>
/// </example>
public interface IInventoryGridAdapter<T> : IInventoryGrid<T>, IHasInventoryGrid<T> where T : class, IInventoryItem
{
    IEnumerable<GridSlot> IInventoryGrid<T>.Items => InventoryGrid.Items;
    bool IInventoryGrid<T>.IsOccupied(Position position) => InventoryGrid.IsOccupied(position);
    bool IInventoryGrid<T>.TryGetItemAt(Position position, out T? item) => InventoryGrid.TryGetItemAt(position, out item);
    bool IInventoryGrid<T>.TryRemoveItemAt(Position position, out T? item) => InventoryGrid.TryRemoveItemAt(position, out item);
    bool IInventoryGrid<T>.TrySetItemAt(Position topLeft, T item) => InventoryGrid.TrySetItemAt(topLeft, item);
    bool IInventoryGrid<T>.TrySetItemAt(Position topLeft, T item, out T? removedItem) => InventoryGrid.TrySetItemAt(topLeft, item, out removedItem);
}