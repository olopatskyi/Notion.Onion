using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using MongoDB.Bson;
using MongoDB.Driver;
using Notion.Domain.Entities;

namespace Notion.MigrationTool.Commands;

[Command(Name = "update", Description = "Update migrations.")]
class UpdateMigrationsCommand
{
    public async Task OnExecute()
    {
        var migrationFiles = Directory.GetFiles("Migrations", "*.cs")
            .OrderBy(file => file)
            .ToList();

        var mongoClient = new MongoClient("mongodb://root:P%40ssw0rd@localhost:27017/");
        var database = mongoClient.GetDatabase("Notion");
        var historyCollection = database.GetCollection<BsonDocument>("MigrationHistory");

        var appliedMigrations = historyCollection.Find(new BsonDocument())
            .ToList()
            .Select(doc => doc["MigrationName"].AsString)
            .ToList();

        foreach (var appliedMigration in appliedMigrations)
        {
            if (!migrationFiles.Any(migrationFile =>
                    Path.GetFileNameWithoutExtension(migrationFile) == appliedMigration))
            {
                // Remove the migration from the history collection
                var filter = Builders<BsonDocument>.Filter.Eq("MigrationName", appliedMigration);
                historyCollection.DeleteOne(filter);

                Console.WriteLine($"Removed migration from history: {appliedMigration}");
            }
        }

        foreach (var migrationFile in migrationFiles)
        {
            var migrationName = Path.GetFileNameWithoutExtension(migrationFile);

            // Check if the migration has already been applied
            if (!appliedMigrations.Contains(migrationName))
            {
                // Apply the migration
                await ApplyMigrationAsync(migrationFile);

                // Record the applied migration in the history collection
                var migrationDocument = new BsonDocument
                {
                    { "MigrationName", migrationName },
                    { "AppliedAt", DateTime.UtcNow }
                };
                historyCollection.InsertOne(migrationDocument);

                Console.WriteLine($"Applied migration: {migrationName}");
            }
        }

        Console.WriteLine("Migration update complete.");
    }

    private async Task ApplyMigrationAsync(string migrationFile)
    {
        Console.WriteLine(migrationFile);
        var migrationAssembly = LoadAssemblyFromSourceFile(migrationFile);
        var migrationType = migrationAssembly.GetTypes()
            .FirstOrDefault(type => typeof(IMigration).IsAssignableFrom(type));

        if (migrationType == null)
        {
            Console.WriteLine($"Migration type not found in {migrationFile}");
            return;
        }

        var migrationInstance = Activator.CreateInstance(migrationType);

        var upMethod = migrationType.GetMethod("UpAsync", BindingFlags.Instance | BindingFlags.Public);

        if (upMethod == null)
        {
            Console.WriteLine($"UpAsync method not found in {migrationType.Name}");
            return;
        }

        // Check if the method returns a Task or Task<T>
        var returnType = upMethod.ReturnType;
        var isTask = returnType == typeof(Task) ||
                     (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>));

        if (!isTask)
        {
            Console.WriteLine($"UpAsync method in {migrationType.Name} must return a Task");
            return;
        }

        // Invoke the UpAsync method
        var task = (Task)upMethod.Invoke(migrationInstance, null);
        await task;

        Console.WriteLine($"Applied migration: {migrationType.Name}");
    }
    
    private Assembly LoadAssemblyFromSourceFile(string sourceFilePath)
    {
        string code = File.ReadAllText(sourceFilePath);
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

        string assemblyName = Path.GetRandomFileName();
        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(MongoClient).Assembly.Location), // Add MongoDB.Driver reference
            MetadataReference.CreateFromFile(typeof(BsonDocument).Assembly.Location), // Add MongoDB.Driver reference
            MetadataReference.CreateFromFile(typeof(ToDoList).Assembly.Location), // Add MongoDB.Driver reference
            MetadataReference.CreateFromFile(typeof(CreateMigrationCommand).Assembly.Location), // Add MongoDB.Driver reference
            MetadataReference.CreateFromFile(typeof(Task).Assembly.Location), // Add MongoDB.Driver reference
          //  MetadataReference.CreateFromFile(typeof(Notion).Assembly.Location) // Add reference to the assembly containing Notion types
        };

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using (MemoryStream ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                Console.WriteLine("Compilation failed:");
                foreach (var diagnostic in result.Diagnostics)
                {
                    Console.WriteLine(diagnostic);
                }

                return null;
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);
                byte[] assemblyBytes = ms.ToArray();
                Assembly assembly = Assembly.Load(assemblyBytes);

                return assembly;
            }
        }
    }

}