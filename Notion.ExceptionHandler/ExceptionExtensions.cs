using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ExceptionHandler;

public static class ExceptionExtensions
{
    private static readonly Type UsedInterface = typeof(IExceptionHandler<>);
    
    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services, params Assembly[] assemblies )
    {
        var constructedServices = GetServices(assemblies);
        
        foreach (var serviceDescriptor in constructedServices)
        {
            services.Add(serviceDescriptor);
        }
        
        return services;
    }

    private static IEnumerable<Type> GetClasses(Assembly[] assemblies)
    {
        return assemblies.SelectMany(a => a.GetTypes())
            .Where(x => x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == UsedInterface));
    }

    private static Dictionary<Type, Type> GetDictionary(Assembly[] assemblies)
    {
        var classes = GetClasses(assemblies);
        var dict = new Dictionary<Type, Type>();
        
        foreach (var clasType in classes)
        {
            var genericArguments = clasType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == UsedInterface)
                .Select(g => g.GetGenericArguments()).FirstOrDefault();

            if (genericArguments == null || genericArguments.Length != 1) 
                continue;
            
            var key = UsedInterface.MakeGenericType(genericArguments);
            dict[key] = clasType;
        }

        return dict;
    }

    private static IEnumerable<ServiceDescriptor> GetServices(Assembly[] assemblies)
    {
        var dict = GetDictionary(assemblies);
        var serviceDescriptors = new List<ServiceDescriptor>();
        
        foreach (var type in dict)
        {
            serviceDescriptors.Add(new ServiceDescriptor(type.Key, type.Value, ServiceLifetime.Singleton));
        }

        return serviceDescriptors;
    }
}