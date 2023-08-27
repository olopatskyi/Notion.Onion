using McMaster.Extensions.CommandLineUtils;

namespace Notion.MigrationTool.Commands;

[Command(Name = "remove", Description = "Remove the last migration.")]
class RemoveMigrationCommand
{
    public void OnExecute()
    {
        var migrationsDirectory = "Migrations";
        var migrationFiles = Directory.GetFiles(migrationsDirectory, "*.cs")
            .OrderByDescending(file => file)
            .ToList();

        if (migrationFiles.Count == 0)
        {
            Console.WriteLine("No migration files found.");
            return;
        }

        var lastMigrationFile = migrationFiles.First();

        // Remove the last migration file
        File.Delete(lastMigrationFile);

        Console.WriteLine($"Removed migration file: {Path.GetFileName(lastMigrationFile)}");
    }
}