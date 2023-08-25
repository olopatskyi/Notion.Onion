using Notion.Application.Models.Request;
using Notion.Application.Models.Response;
using Notion.Domain.Entities;

namespace Notion.Application.Interfaces;

public interface ITodoListService
{
    Task CreateAsync(string userId, CreateToDoList model);
    
    Task<IEnumerable<GetAllToDoListResponse>> GetAsync(string userId, GetToDoLists model);
    
    Task<ToDoList?> GetByIdAsync(string userId, string taskListId);
    
    Task DeleteAsync(string userId, string toDoListId);

    Task UpdateAsync(string userId, UpdateToDoList model);
}