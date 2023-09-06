using System.Net;

namespace AspNetCoreFromScratch;

public class CustomMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpListenerContext context, Func<Task> next)
    {
        Console.WriteLine("Before invoking next");
        await next();
        Console.WriteLine("After invoking next");
    }
}