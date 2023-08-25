using Notion.Domain.Entities;

namespace Notion.Domain.Interface;

public interface IToDoItemRepository : IRepository<ToDoItem>
{
    Task UpdateToDoItemAsync(string listId, string itemId, ToDoItem updatedItem);
}