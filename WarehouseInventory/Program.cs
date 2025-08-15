using System;
using System.Collections.Generic;

interface IInventoryItem { int Id { get; } string Name { get; } int Quantity { get; set; } }

class ElectronicItem : IInventoryItem
{
    public int Id { get; } public string Name { get; } public int Quantity { get; set; }
    public string Brand; public int WarrantyMonths;
    public ElectronicItem(int id, string name, int qty, string brand, int warranty)
    { Id = id; Name = name; Quantity = qty; Brand = brand; WarrantyMonths = warranty; }
}

class GroceryItem : IInventoryItem
{
    public int Id { get; } public string Name { get; } public int Quantity { get; set; }
    public DateTime ExpiryDate;
    public GroceryItem(int id, string name, int qty, DateTime expiry)
    { Id = id; Name = name; Quantity = qty; ExpiryDate = expiry; }
}

class DuplicateItemException : Exception { public DuplicateItemException(string m) : base(m) { } }
class ItemNotFoundException : Exception { public ItemNotFoundException(string m) : base(m) { } }
class InvalidQuantityException : Exception { public InvalidQuantityException(string m) : base(m) { } }

class InventoryRepository<T> where T : IInventoryItem
{
    Dictionary<int, T> _items = new();
    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id)) throw new DuplicateItemException("Item already exists");
        _items[item.Id] = item;
    }
    public T GetItemById(int id) =>
        _items.ContainsKey(id) ? _items[id] : throw new ItemNotFoundException("Item not found");
    public void RemoveItem(int id)
    {
        if (!_items.Remove(id)) throw new ItemNotFoundException("Item not found");
    }
    public List<T> GetAllItems() => new(_items.Values);
    public void UpdateQuantity(int id, int qty)
    {
        if (qty < 0) throw new InvalidQuantityException("Quantity cannot be negative");
        GetItemById(id).Quantity = qty;
    }
}

class WarehouseManager
{
    InventoryRepository<ElectronicItem> _electronics = new();
    InventoryRepository<GroceryItem> _groceries = new();

    public void SeedData()
    {
        _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "Dell", 24));
        _electronics.AddItem(new ElectronicItem(2, "Phone", 10, "Samsung", 12));
        _groceries.AddItem(new GroceryItem(1, "Apple", 20, DateTime.Now.AddDays(7)));
        _groceries.AddItem(new GroceryItem(2, "Milk", 15, DateTime.Now.AddDays(3)));
    }

    public void PrintAll<T>(InventoryRepository<T> repo) where T : IInventoryItem =>
        repo.GetAllItems().ForEach(i => Console.WriteLine($"{i.Id} - {i.Name} ({i.Quantity})"));

    static void Main()
    {
        var manager = new WarehouseManager();
        try
        {
            manager.SeedData();
            Console.WriteLine("Electronics:"); manager.PrintAll(manager._electronics);
            Console.WriteLine("Groceries:"); manager.PrintAll(manager._groceries);

            // Exception tests
            manager._electronics.AddItem(new ElectronicItem(1, "Tablet", 3, "Lenovo", 12));
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

        try { manager._groceries.RemoveItem(99); }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

        try { manager._groceries.UpdateQuantity(1, -5); }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }
}

