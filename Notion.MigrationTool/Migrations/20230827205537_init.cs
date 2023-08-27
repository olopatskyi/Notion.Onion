using MongoDB.Driver;
using MongoDB.Bson;
using Notion.Domain.Entities;
using Notion.MigrationTool;

namespace YourNamespace.Migrations
{
    public class Migration_20230827205537_init : IMigration
    {
        public async Task UpAsync(IMongoDatabase database)
        {
            var collecion = database.GetCollection<ToDoList>("ToDoList");

            await collecion.InsertOneAsync(new ToDoList()
            {
                Title = "Test",
                OwnerId = Guid.NewGuid().ToString(),
                Items = new List<ToDoItem>()
            });
        }

        public async Task DownAsync(IMongoDatabase database)
        {
            var collecion = database.GetCollection<ToDoList>("ToDoList");

            await collecion.DeleteOneAsync(Builders<ToDoList>.Filter.Eq("Title", "Test"));
        }
    }
}
