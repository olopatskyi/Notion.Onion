using MongoDB.Bson;
using MongoDB.Driver;
using Notion.Domain.Entities;
using Notion.Domain.Interface;
using Notion.Domain.Shared;

namespace Notion.Infrastructure.Repository;

public class ToDoListRepository : Repository<ToDoList>, IToDoListRepository
{
    public ToDoListRepository(IMongoClient client, DatabaseSettings databaseSettings) : base(client, databaseSettings)
    {
    }

    public async Task<IEnumerable<ToDoList>> GetByFilterDefinitionAsync(FilterDefinition<ToDoList> filterDefinition,
        SortDefinition<ToDoList> sortDefinition, int pageSize,
        int pageNumber)
    {
        int skipAmount = (pageNumber - 1) * pageSize;

        var toDoLists = await Collection.Find(filterDefinition)
            .Sort(sortDefinition)
            .Skip(skipAmount)
            .Limit(pageSize)
            .ToListAsync();

        return toDoLists;
    }
}