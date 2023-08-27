using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Notion.Domain.Interface;
using Notion.Domain.Shared;
using Notion.Infrastructure.Repository;

namespace Notion.Domain.DI;

public static class DbContextExtensions
{
    public static IServiceCollection AddMongoConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(provider =>
        {
            var section = configuration.GetSection("DatabaseSettings");
            var server = section["Server"];
            var client = new MongoClient(server);
            return client;
        });

        return services;
    }

    public static IServiceCollection AddRepository(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        serviceCollection.AddScoped<IToDoListRepository, ToDoListRepository>();
        return serviceCollection;
    }
}