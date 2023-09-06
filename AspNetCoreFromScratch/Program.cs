using System.Net;
using AspNetCoreFromScratch;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = new ServiceCollection()
    .AddSingleton<RouteRegistry>()
    .AddControllers()
    .AddMiddleware<CustomMiddleware>()
    .AddMiddleware<RoutingMiddleware>()
    .BuildServiceProvider();

// Get the list of middleware from the DI container
var middlewares = serviceProvider.GetServices<IMiddleware>().ToList();

// Create a middleware container
var middlewareContainer = new MiddlewarePipeline(middlewares);

var httpListener = new HttpListener();
httpListener.Prefixes.Add("http://localhost:5001/");
httpListener.Start();

Console.WriteLine("Listening...");

while (true)
{
    var context = httpListener.GetContext();
    await middlewareContainer.InvokeAsync(context);
    context.Response.Close();
}