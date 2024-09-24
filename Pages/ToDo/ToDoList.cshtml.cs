using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Pages.ToDo;

public class ToDoListModel(IToDoService toDoService) : PageModel
{
    private readonly IToDoService _toDoService = toDoService;
    public IEnumerable<ToDoItem>? ToDoItems { get; private set; }
    public ToDoItem? EditingItem { get; set; } = null;

    public async Task<IActionResult> OnGetAsync()
    {
        ToDoItems = await _toDoService.GetAllAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string title, string details)
    {
        var newItem = new ToDoItem { Title = title, Details = details };
        await _toDoService.CreateAsync(newItem);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleStatusAsync(string id)
    {
        var item = await _toDoService.GetByIdAsync(id);
        if (item != null)
        {
            item.IsCompleted = !item.IsCompleted;
            await _toDoService.UpdateAsync(id, item);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        await _toDoService.DeleteAsync(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync(string id)
    {
        EditingItem = await _toDoService.GetByIdAsync(id);
        ToDoItems = await _toDoService.GetAllAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateAsync(string editingId, string title, string details, bool iscompleted)
    {
        var updatedItem = new ToDoItem { Id = editingId, Title = title, Details = details, IsCompleted = iscompleted };
        await _toDoService.UpdateAsync(editingId, updatedItem);
        return RedirectToPage();
    }

}