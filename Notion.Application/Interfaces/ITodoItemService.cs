using Notion.Application.Models.Request;

namespace Notion.Application.Interfaces;

public interface ITodoItemService
{
    Task CreateAsync(string userId, string toDoListId, CreateToDoItem model);

    Task UpdateAsync(string userId, string toDoListId, string title, UpdateToDoItem model);
}