using System.Net;
using System.Text;
using System.Text.Json;

namespace AspNetCoreFromScratch;

public class RoutingMiddleware : IMiddleware
{
    private readonly RouteRegistry _routeRegistry;
    private readonly IServiceProvider _serviceProvider;

    public RoutingMiddleware(RouteRegistry routeRegistry, IServiceProvider serviceProvider)
    {
        _routeRegistry = routeRegistry;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpListenerContext context, Func<Task> next)
    {
        Console.WriteLine("Inside RoutingMiddleware");
        if (_routeRegistry.Routes.TryGetValue(context.Request.RawUrl[1..], out var controllerAction))
        {
            // Read the request body and deserialize it to the appropriate type.
            using var reader = new StreamReader(context.Request.InputStream);
            var requestBody = await reader.ReadToEndAsync();

            // The type of object to deserialize to is determined by the method's first parameter.
            var parameterType = controllerAction.Method.GetParameters()[0].ParameterType;
            var requestObj = JsonSerializer.Deserialize(requestBody, parameterType);

            // Fetch the controller from the DI container.
            var controllerInstance = _serviceProvider.GetService(controllerAction.Controller);

            // Invoke the controller method and get the result.
            var actionResult = controllerAction.Method.Invoke(controllerInstance, new[] { requestObj });

            // The type of object to serialize is determined by the method's return type.
            var resultJson = JsonSerializer.Serialize(actionResult);
            
            // Write the serialized result back to the response stream.
            await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(resultJson));
        }
        else
        {
            // Short-circuit the pipeline, handle not found.
            context.Response.StatusCode = 404;
            await context.Response.OutputStream.WriteAsync("Not Found"u8.ToArray());
        }
    }
}