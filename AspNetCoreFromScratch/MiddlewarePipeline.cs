using System.Net;
using AspNetCoreFromScratch;
using Microsoft.Extensions.DependencyInjection;

public class MiddlewarePipeline
{
    private readonly IReadOnlyList<IMiddleware> _middlewares;

    public MiddlewarePipeline(IReadOnlyList<IMiddleware> middlewares)
    {
        _middlewares = middlewares;
    }

    public Task InvokeAsync(HttpListenerContext context)
    {
        var index = -1;

        Func<Task>? nextMiddleware = null;
        nextMiddleware = () =>
        {
            index++;
            // If there are no more middlewares, return a completed task.
            // Otherwise, invoke the next middleware.
            return index < _middlewares.Count 
                ? _middlewares[index].InvokeAsync(context, nextMiddleware) 
                : Task.CompletedTask;
        };

        return nextMiddleware();
    }
}
