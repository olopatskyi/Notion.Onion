using MongoDB.Driver;
namespace Notion.MigrationTool;

public interface IMigration
{
   Task ExecuteAsync(IMongoDatabase database);

   Task RevertAsync(IMongoDatabase database);
}