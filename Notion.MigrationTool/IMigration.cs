using MongoDB.Driver;
namespace Notion.MigrationTool;

public interface IMigration
{
    Task UpAsync(IMongoDatabase database);

    Task DownAsync(IMongoDatabase database);
}