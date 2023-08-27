using McMaster.Extensions.CommandLineUtils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Notion.MigrationTool.Commands;

[Command(Name = "create", Description = "Create a new migration.")]
class CreateMigrationCommand
{
    [Argument(0, "MigrationName", Description = "The name of the migration.")]
    public string MigrationName { get; }

    public void OnExecute()
    {
        var migrationsDirectory = "Migrations";
        if (!Directory.Exists(migrationsDirectory))
        {
            Directory.CreateDirectory(migrationsDirectory);
        }

        // Create a migration timestamp
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        // Construct the migration file name
        var migrationFileName = $"{timestamp}_{MigrationName}.cs";

        // Construct the migration file content
        var migrationContent = GetMigrationContent(timestamp, MigrationName);

        // Create the migration file in the migrations directory
        var migrationFilePath = Path.Combine(migrationsDirectory, migrationFileName);
        File.WriteAllText(migrationFilePath, migrationContent);
        
        RecordAppliedMigration(migrationFileName);
        Console.WriteLine($"Created migration file: {migrationFileName}");
    }

    private string GetMigrationContent(string timestamp, string migrationName)
    {
        return @$"using MongoDB.Driver;
using MongoDB.Bson;
using Notion.MigrationTool;

namespace YourNamespace.Migrations
{{
    public class Migration_{timestamp}_{migrationName} : IMigration
    {{
        public Task UpAsync(IMongoDatabase database)
        {{
            // Your migration logic for applying changes
        }}

        public Task DownAsync(IMongoDatabase database)
        {{
            // Your migration logic for reverting changes
        }}
    }}
}}
";
    }
    
    private void RecordAppliedMigration(string migrationName)
    {
        var mongoClient = new MongoClient("mongodb://root:P%40ssw0rd@localhost:27017/");
        var database = mongoClient.GetDatabase("Notion");
        var historyCollection = database.GetCollection<BsonDocument>("MigrationHistory");

        var migrationDocument = new BsonDocument
        {
            { "MigrationName", migrationName },
            { "AppliedAt", DateTime.UtcNow }
        };

        historyCollection.InsertOne(migrationDocument);
    }
}