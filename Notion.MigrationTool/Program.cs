using McMaster.Extensions.CommandLineUtils;
using Notion.MigrationTool.Commands;

namespace MongoMigrationTool
{ 
    [Command(Name = "mongo-migrate", Description = "A simple migration tool for MongoDB.")]
    [Subcommand(typeof(CreateMigrationCommand))]
    [Subcommand(typeof(RemoveMigrationCommand))]
    [Subcommand(typeof(UpdateMigrationsCommand))]
    class Program
    {
        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        private void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }

    [Command(Name = "apply", Description = "Apply migrations.")]
    class ApplyMigrationsCommand
    {
        public void OnExecute()
        {
            // Your logic to apply migrations
            Console.WriteLine("Applying migrations...");
        }
    }

    [Command(Name = "rollback", Description = "Rollback migrations.")]
    class RollbackMigrationsCommand
    {
        public void OnExecute()
        {
            // Your logic to rollback migrations
            Console.WriteLine("Rolling back migrations...");
        }
    }
}