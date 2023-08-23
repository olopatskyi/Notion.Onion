using System.Reflection;
using Notion.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Notion.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        return services.AddAutoMapper(typeof(TodoListService));
    }
}