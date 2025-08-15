using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

interface IInventoryEntity { int Id { get; } }
record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

class InventoryLogger<T> where T : IInventoryEntity
{
    List<T> _log = new();
    string _filePath = "inventory.json";

    public void Add(T item) => _log.Add(item);
    public List<T> GetAll() => _log;

    public void SaveToFile()
    {
        try
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_log));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    public void LoadFromFile()
    {
        try
        {
            if (File.Exists(_filePath))
                _log = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(_filePath)) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
        }
    }
}

class InventoryApp
{
    InventoryLogger<InventoryItem> _logger = new();

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Pen", 100, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Book", 50, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Notebook", 75, DateTime.Now));
    }

    public void SaveData() => _logger.SaveToFile();
    public void LoadData() => _logger.LoadFromFile();

    public void PrintAllItems()
    {
        foreach (var item in _logger.GetAll())
            Console.WriteLine($"{item.Id} - {item.Name} ({item.Quantity}) added on {item.DateAdded}");
    }

    static void Main()
    {
        var app = new InventoryApp();

        // First run: seed and save
        app.SeedSampleData();
        app.SaveData();
        Console.WriteLine("Data saved. Simulating new session...\n");

        // Simulate fresh session by creating a new instance
        var newApp = new InventoryApp();
        newApp.LoadData();
        Console.WriteLine("Loaded Data:");
        newApp.PrintAllItems();
    }
}

