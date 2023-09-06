using System.Net;

namespace AspNetCoreFromScratch;

public interface IMiddleware
{
    Task InvokeAsync(HttpListenerContext context, Func<Task> next);
}