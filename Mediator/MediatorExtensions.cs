using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        // We allow multiple assemblies to scan - if the user doesn't add anything, we assume the current one
        // where we are looking for INotificationHandler<T>
        if (assemblies.Length == 0)
        {
            assemblies = new[] { Assembly.GetExecutingAssembly() };
        }

        services.AddSingleton<IMediator, Mediator>();

        // Register all handler types found in the specified assemblies.
        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.ExportedTypes
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                .ToList();

            foreach (var handlerType in handlerTypes)
            {
                services.AddTransient(handlerType);
            }
        }

        // Build the handler registry using the registered handlers.
        services.AddSingleton<NotificationHandlerRegistry>(provider =>
        {
            var registry = new NotificationHandlerRegistry();

            foreach (var service in services)
            {
                if (service.ServiceType.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                {
                    // Get the INotificationHandler<T> instances from the container
                    var handler = provider.GetServices(service.ServiceType);
                    foreach (var h in handler.Where(s => s is not null))
                    {
                        var handlerInterface = h!.GetType().GetInterfaces().First();
                        var messageType = handlerInterface.GetGenericArguments().First();
                        typeof(NotificationHandlerRegistry)
                            .GetMethod("AddHandler")!
                            .MakeGenericMethod(messageType)
                            .Invoke(registry, new[] { h });
                    }
                }
            }

            return registry;
        });

        return services;
    }
}