using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Services;
public interface IToDoService
{
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem> GetByIdAsync(string id);
    Task<ToDoItem> CreateAsync(ToDoItem item);
    Task UpdateAsync(string id, ToDoItem item);
    Task DeleteAsync(string id);
}
