using MongoDB.Driver;

namespace Notion.MigrationTool;

public class MigrationRunner
{
    private readonly IMongoDatabase _database;

    public MigrationRunner(IMongoDatabase database)
    {
        _database = database;
    }
    
    
}