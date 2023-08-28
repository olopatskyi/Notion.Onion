using MongoDB.Driver;
using Notion.MigrationTool.Migrations;

namespace Notion.MigrationTool
{
   internal class Program
   {
      public static async Task Main(string[] args)
      {
         var client = new MongoClient("mongodb://root:P%40ssw0rd@localhost:27017/");
         IMigrationExecutor executor = new MigrationExecutor(client);
         await executor.ApplyAsync(new ContributorIndexes());
      }
   }
}