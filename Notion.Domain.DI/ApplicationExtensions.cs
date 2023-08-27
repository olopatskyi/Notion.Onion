using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Notion.Application.EventHandlers.ToDoListEventHandler;
using Notion.Application.Factories;
using Notion.Application.Interfaces;
using Notion.Application.Services;
using Notion.Domain.Shared;

namespace Notion.Domain.DI;

public static class ApplicationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITodoListService, TodoListService>();
        serviceCollection.AddScoped<ITodoItemService, TodoItemService>();
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

    public static IServiceCollection AddEventHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ObserverAndHandlerFactory>(); // Register the factory
        serviceCollection.AddSingleton(provider =>
            provider.GetRequiredService<ObserverAndHandlerFactory>().CreateObserver()); // Create singleton observer
        serviceCollection.AddSingleton(provider =>
            provider.GetRequiredService<ObserverAndHandlerFactory>()
                .CreateEventHandler()); // Create singleton event handler

        return serviceCollection;
    }
}