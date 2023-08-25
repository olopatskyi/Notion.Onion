using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Notion.Domain.Entities;
using Notion.Domain.Interface;
using Notion.Domain.Shared;

namespace Notion.Infrastructure.Repository;

public class ToDoItemRepository : Repository<ToDoItem>, IToDoItemRepository
{
    public ToDoItemRepository(IMongoClient client, DatabaseSettings databaseSettings) : base(client, databaseSettings)
    {
    }


    public Task UpdateToDoItemAsync(string listId, string itemId, ToDoItem updatedItem)
    {
        throw new NotImplementedException();
    }
}