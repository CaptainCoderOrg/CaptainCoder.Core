namespace Inventory;
using CaptainCoder.Inventory;

public class SimpleInventoryGridTest
{
    private static readonly MockInventoryItem s_Dagger = new("Dagger", new Dimensions(2, 1));
    private static readonly MockInventoryItem s_Chest = new("Chest", new Dimensions(3, 2));
    private static readonly MockInventoryItem s_Shield = new("Shield", new Dimensions(2, 2));


    [Theory]
    [InlineData(4, 4)]
    [InlineData(1, 10)]
    [InlineData(10, 7)]
    [InlineData(100, 100)]
    public void TestConstructor(int rows, int cols)
    {
        SimpleInventoryGrid<MockInventoryItem> inventory = new() { GridSize = new Dimensions(rows, cols) };
        Assert.Equal(new Dimensions(rows, cols), inventory.GridSize);
        Assert.True(inventory.Items.ToList() is []);
    }

    [Fact]
    public void TestSimpleAddAndRemove()
    {
        SimpleInventoryGrid<MockInventoryItem> inventory = new() { GridSize = new Dimensions(4, 10) };
        Assert.True(inventory.TrySetItemAt((0, 0), s_Dagger));
        Assert.Single(inventory.Items);
        Assert.Contains(new IInventoryGrid<MockInventoryItem>.GridSlot((0, 0), s_Dagger, inventory), inventory.Items);
        Assert.True(inventory.IsOccupied((0, 0)));
        Assert.True(inventory.IsOccupied((1, 0)));
        Assert.True(inventory.TryGetItemAt((0, 0), out MockInventoryItem? item));
        Assert.Equal(s_Dagger, item);
        Assert.True(inventory.TryGetItemAt((1, 0), out item));
        Assert.Equal(s_Dagger, item);
        Assert.True(inventory.TryRemoveItemAt((0, 0), out item));
        Assert.Equal(s_Dagger, item);
        Assert.Empty(inventory.Items);
        Assert.False(inventory.IsOccupied((0, 0)));
        Assert.False(inventory.IsOccupied((1, 0)));
        Assert.False(inventory.TryRemoveItemAt((0, 0), out _));
        Assert.True(inventory.TrySetItemAt((0, 0), s_Dagger));
        Assert.True(inventory.TryRemoveItemAt((1, 0), out item));
        Assert.Equal(s_Dagger, item);
        Assert.Empty(inventory.Items);
        Assert.False(inventory.IsOccupied((0, 0)));
        Assert.False(inventory.IsOccupied((1, 0)));
    }

    [Fact]
    public void TestAddOutOfBounds()
    {
        SimpleInventoryGrid<MockInventoryItem> inventory = new() { GridSize = new Dimensions(4, 10) };
        Assert.False(inventory.TrySetItemAt((-1, 0), s_Dagger));
        Assert.False(inventory.TrySetItemAt((3, 9), s_Dagger));
        Assert.False(inventory.TrySetItemAt((4, 0), s_Dagger));
        Assert.False(inventory.TrySetItemAt((0, 10), s_Dagger));
    }

    [Fact]
    public void TestSimpleAddWithSwap()
    {
         SimpleInventoryGrid<MockInventoryItem> inventory = new() { GridSize = new Dimensions(4, 10) };
         Assert.True(inventory.TrySetItemAt((0,0), s_Dagger));
         Assert.True(inventory.TrySetItemAt((0,0), s_Chest, out MockInventoryItem? removedItem));
         Assert.Equal(s_Dagger, removedItem);
         Assert.Single(inventory.Items);
         var chestSlot = new IInventoryGrid<MockInventoryItem>.GridSlot((0,0), s_Chest, inventory);
         Assert.Contains(chestSlot, inventory.Items);
         Assert.True(inventory.TrySetItemAt((0,2), s_Dagger));
         /* 
            AAD.
            AAD.
            AA..
            ....
         */
         Assert.False(inventory.TrySetItemAt((0,1), s_Shield, out _));
         Assert.Equal(2, inventory.Items.Count());
         var daggerSlot = new IInventoryGrid<MockInventoryItem>.GridSlot((0,2), s_Dagger, inventory);
         Assert.Contains(chestSlot, inventory.Items);
         Assert.Contains(daggerSlot, inventory.Items);
    }
}