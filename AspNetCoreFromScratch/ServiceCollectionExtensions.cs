using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreFromScratch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddControllers(this IServiceCollection services)
    {
        var controllers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ControllerBase)));

        foreach (var controller in controllers)
        {
            services.AddSingleton(controller);
        }

        return services;
    }
    
    public static IServiceCollection AddMiddleware<TMiddleware>(this IServiceCollection services)
        where TMiddleware : class, IMiddleware
    {
        services.AddSingleton<IMiddleware, TMiddleware>();
        return services;
    }
}