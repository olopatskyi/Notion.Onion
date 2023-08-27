using MongoDB.Driver;
using Notion.Domain.Entities;

namespace Notion.Domain.Interface;

public interface IToDoListRepository : IRepository<ToDoList>
{
    Task<IEnumerable<ToDoList>> GetByFilterDefinitionAsync(FilterDefinition<ToDoList> filterDefinition,
        SortDefinition<ToDoList> sortDefinition, int pageSize, int pageNumber);
}