using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PerformanceNet10;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class AspNetBenchmarks
{
    private HttpClient _minimalApiClient;
    private WebApplication _minimalApiApp;

    [Benchmark]
    public async Task<List<ReturnValue>?> GetTwoTimes100InParallelMinimalApi()
    {
        var tasks = new List<Task<ReturnValue?>>();
        for (var i = 0; i < 100; i++)
        {
            tasks.Add(_minimalApiClient.GetFromJsonAsync<ReturnValue?>("/simple?a=1&b=2"));
        }
    
        await Task.WhenAll(tasks);
    
        for (var i = 0; i < 100; i++)
        {
            tasks.Add(_minimalApiClient.GetFromJsonAsync<ReturnValue?>("/simple?a=1&b=2"));
        }
    
        await Task.WhenAll(tasks);
        return tasks.Select(t => t.Result).Select(s => s).ToList();
    }
    [GlobalSetup]
    public void Setup()
    {
        _minimalApiApp = CreateMinimalApiApp();

        _minimalApiClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:1234")
        };
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _minimalApiApp.StopAsync().GetAwaiter().GetResult();
        _minimalApiClient.Dispose();
    }

    private static WebApplication CreateMinimalApiApp()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddScoped<Service>();
        builder.Logging.ClearProviders();
        var minimalApiApp = builder.Build();
    
        // Add a correlation id header if not present!
        minimalApiApp.Use((context, next) =>
        {
            if (!context.Request.Headers.ContainsKey("Correlation-Id"))
            {
                context.Request.Headers.Append("Correlation-Id", Guid.NewGuid().ToString());
            }
    
            return next(context);
        });

        // Check for an exception and return a 500 response if one is thrown
        minimalApiApp.Use((context, next) =>
        {
            try
            {
                return next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                return context.Response.WriteAsync(e.Message);
            }
        });
        minimalApiApp.MapGet("/simple", ([FromQuery]string a, [FromQuery]string b, Service service) => 
        service.Get(a, b));
        minimalApiApp.RunAsync("http://localhost:1234");
        return minimalApiApp;
    }
}

public sealed record ReturnValue
{
    private static readonly List<int> PreNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
    
    public required string Property { get; init; }
    public string Property2 { get; init; } = "Some Value Here that is not used directly but will be part of the output JSON";
    public IReadOnlyCollection<int> Numbers { get; init; } = PreNumbers;
    public ReturnValue? Child { get; init; }
}

public class Service
{
    public ReturnValue Get(string a, string b) =>
        new()
        {
            Property = a,
            Child = new ReturnValue
            {
                Property = b
            }
        };
}
