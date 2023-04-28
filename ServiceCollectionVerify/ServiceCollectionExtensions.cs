using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceCollectionVerify;

public class VerifyResult
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Found {Errors.Count} error(s):");
        for (var i = 0; i < Errors.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {Errors[i]}");
        }

        return sb.ToString();
    }
}

public static class ServiceCollectionExtensions
{
    public static void Verify(this IServiceCollection services)
    {
var result = new VerifyResult();
using var serviceProvider = services.BuildServiceProvider();

result.Errors.AddRange(CheckServicesCanBeResolved(serviceProvider, services));
result.Errors.AddRange(CheckForCaptiveDependencies(services));

if (!result.IsValid)
{
    throw new InvalidOperationException(result.ToString());
}
    }

    private static List<string> CheckServicesCanBeResolved(IServiceProvider serviceProvider, IServiceCollection services)
    {
        var unresolvedTypes = new List<string>();
        foreach (var serviceDescriptor in services)
        {
            try
            {
                serviceProvider.GetRequiredService(serviceDescriptor.ServiceType);
            }
            catch
            {
                unresolvedTypes.Add($"Unable to resolve '{serviceDescriptor.ServiceType.FullName}'");
            }
        }

        return unresolvedTypes;
    }

private static IEnumerable<string> CheckForCaptiveDependencies(IServiceCollection services)
{
    var singletonServices = services
        .Where(descriptor => descriptor.Lifetime == ServiceLifetime.Singleton)
        .Select(descriptor => descriptor.ServiceType);

    foreach (var singletonService in singletonServices)
    {
        var captiveScopedServices = singletonService
            .GetConstructors()
            .SelectMany(property => property.GetParameters())
            .Where(propertyType => services.Any(descriptor => descriptor.ServiceType == propertyType.ParameterType
                                                              && descriptor.Lifetime == ServiceLifetime.Scoped
                                                              || descriptor.Lifetime == ServiceLifetime.Transient));

        foreach (var captiveService in captiveScopedServices)
        {
            yield return $"Singleton service '{singletonService.FullName}' has one or more captive dependencies: {string.Join(", ", captiveService.ParameterType.FullName)}";
        }
    }
}
}