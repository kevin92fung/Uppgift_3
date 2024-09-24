using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Services;

public class JsonToDoService : IToDoService
{
    private readonly string _jsonFilePath = "todos.json";

    public async Task<IEnumerable<ToDoItem>> GetAllAsync()
    {
        var items = await ReadJsonFileAsync();
        return items;
    }

    public async Task<ToDoItem> GetByIdAsync(string id)
    {
        var items = await ReadJsonFileAsync();
        return items.FirstOrDefault(item => item.Id == id) ?? throw new KeyNotFoundException($"ToDo item with Id '{id}' not found.");
    }

    public async Task<ToDoItem> CreateAsync(ToDoItem item)
    {
        var items = await ReadJsonFileAsync();
        item.Id = System.Guid.NewGuid().ToString();
        items.Add(item);
        await WriteJsonFileAsync(items);
        return item;
    }

    public async Task UpdateAsync(string id, ToDoItem updatedItem)
    {
        var items = await ReadJsonFileAsync();
        var itemIndex = items.FindIndex(item => item.Id == id);
        if (itemIndex != -1)
        {
            updatedItem.Id = id;
            items[itemIndex] = updatedItem;
            await WriteJsonFileAsync(items);
        }
        else
        {
            throw new KeyNotFoundException($"ToDo item with Id '{id}' not found.");
        }
    }

    public async Task DeleteAsync(string id)
    {
        var items = await ReadJsonFileAsync();
        var item = items.FirstOrDefault(item => item.Id == id);
        if (item != null)
        {
            items.Remove(item);
            await WriteJsonFileAsync(items);
        }
        else
        {
            throw new KeyNotFoundException($"ToDo item with Id '{id}' not found.");
        }
    }

    private async Task<List<ToDoItem>> ReadJsonFileAsync()
    {
        if (!File.Exists(_jsonFilePath))
        {
            File.Create(_jsonFilePath).Dispose();
            return new List<ToDoItem>();
        }

        var json = await File.ReadAllTextAsync(_jsonFilePath);
        var items = JsonSerializer.Deserialize<List<ToDoItem>>(json);
        if (items == null)
        {
            items = new List<ToDoItem>();
            items.Add(new ToDoItem { Title = "Learn ASP.NET", Details = "Learn Razor Pages to build interactive web UIs using C#" });
        }
        return items;
    }


    private async Task WriteJsonFileAsync(List<ToDoItem> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_jsonFilePath, json);
    }

}

