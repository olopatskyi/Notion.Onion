using MongoDB.Driver;

namespace Notion.MigrationTool;

public class MigrationExecutor : IMigrationExecutor
{
    private readonly IMongoDatabase _database;
    public MigrationExecutor(IMongoClient client)
    {
        _database = client.GetDatabase("Notion");
    }
    public async Task ApplyAsync(IMigration migration)
    {
        Console.WriteLine("Applying migration...");
        await migration.ExecuteAsync(_database);
        Console.WriteLine("Migration has been applied");
    }

    public async Task RollbackAsync(IMigration migration)
    {
        Console.WriteLine("Rolling back migration...");
        await migration.RevertAsync(_database);
        Console.WriteLine("Migration has been rolled back");
    }
}