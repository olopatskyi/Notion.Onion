using MongoDB.Driver;
using Notion.Domain.Entities;

namespace Notion.MigrationTool.Migrations;

public class ContributorIndexes : IMigration
{
    public async Task ExecuteAsync(IMongoDatabase database)
    {
        var collection = database.GetCollection<Contributor>(nameof(Contributor));


        var indexKeysDefinition = Builders<Contributor>.IndexKeys
            .Ascending(x => x.UserId)
            .Ascending(x => x.ListId); // Assuming ListId is the field corresponding to the ListId you mentioned

        var indexOptions = new CreateIndexOptions { Unique = false }; // Modify uniqueness based on your needs
        var model = new CreateIndexModel<Contributor>(indexKeysDefinition, indexOptions);

        await collection.Indexes.CreateOneAsync(model);
    }

    public async Task RevertAsync(IMongoDatabase database)
    {
        var collection = database.GetCollection<Contributor>(nameof(Contributor));

        var indexName = "UserId_1_ListId_1"; // Name of the index
        await collection.Indexes.DropOneAsync(indexName);
    }
}