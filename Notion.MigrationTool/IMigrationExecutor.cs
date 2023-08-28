namespace Notion.MigrationTool;

public interface IMigrationExecutor
{
    Task ApplyAsync(IMigration migration);
    
    Task RollbackAsync(IMigration migration);
}