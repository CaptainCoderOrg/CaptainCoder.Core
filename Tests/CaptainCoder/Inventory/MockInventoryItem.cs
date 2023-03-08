namespace CaptainCoder.Inventory;

internal record class MockInventoryItem(string Name, Dimensions Size) : IInventoryItem;