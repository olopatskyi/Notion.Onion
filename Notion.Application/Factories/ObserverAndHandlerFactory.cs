using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Notion.Application.EventHandlers;
using Notion.Application.EventHandlers.ToDoListEventHandler;
using Notion.Domain.Entities;
using Notion.Domain.Interface;

namespace Notion.Application.Factories;

public class ObserverAndHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ObserverAndHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ToDoListObserver CreateObserver()
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Contributor>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        return new ToDoListObserver(repository, mapper);
    }

    public ToDoListEventHandler CreateEventHandler()
    {
        using var scope = _serviceProvider.CreateScope();
        var observer = CreateObserver();
        return new ToDoListEventHandler(observer);
    }
}
