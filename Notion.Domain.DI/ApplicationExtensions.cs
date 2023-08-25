using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Notion.Application.Interfaces;
using Notion.Application.Services;
using Notion.Domain.Shared;

namespace Notion.Domain.DI;

public static class ApplicationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ITodoListService, TodoListService>();
        serviceCollection.AddTransient<ITodoItemService, TodoItemService>();
        return serviceCollection;
    }

    public static IServiceCollection AddOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));

        serviceCollection.AddSingleton(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<DatabaseSettings>>();
            return settings.Value;
        });

        return serviceCollection;
    }
}