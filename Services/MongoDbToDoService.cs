using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Services;
public class MongoDbToDoService : IToDoService
{
    private readonly IMongoCollection<ToDoItem> _toDoItems;

    public MongoDbToDoService(IConfiguration config)
    {
        var mongoDbSettings = config.GetSection("MongoDbSettings");
        var client = new MongoClient(mongoDbSettings["ConnectionString"]);
        var database = client.GetDatabase(mongoDbSettings["DatabaseName"]);
        _toDoItems = database.GetCollection<ToDoItem>(mongoDbSettings["CollectionName"]);
    }

    public async Task<ToDoItem> CreateAsync(ToDoItem newItem)
    {
        await _toDoItems.InsertOneAsync(newItem);
        return newItem;
    }

    public async Task DeleteAsync(string id)
    {
        await _toDoItems.DeleteOneAsync(item => item.Id == id);
    }

    public async Task<ToDoItem> GetByIdAsync(string id)
    {
        return await _toDoItems.Find(item => item.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ToDoItem>> GetAllAsync()
    {
        return await _toDoItems.Find(_ => true).ToListAsync();
    }

    public async Task UpdateAsync(string id, ToDoItem updatedItem)
    {
        await _toDoItems.ReplaceOneAsync(item => item.Id == id, updatedItem);
    }
}
