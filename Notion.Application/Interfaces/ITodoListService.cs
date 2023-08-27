using Notion.Application.Models.Request;
using Notion.Application.Models.Response;
using Notion.Domain.Entities;

namespace Notion.Application.Interfaces;

public interface ITodoListService
{
    Task CreateAsync(string userId, CreateToDoList model);
    
    Task<IEnumerable<GetAllToDoListResponse>> GetAsync(string userId, GetToDoLists model);
    
    Task<GetToDoListResponse> GetByIdAsync(string userId, string listId);
    
    Task DeleteAsync(string userId, string toDoListId);

    Task UpdateAsync(string userId, UpdateToDoList model);

    Task AddContributorAsync(string userId, string listId, AddContributorRequest model);
}