using WebAppFactory;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/", (HelloWorldRequest request) => $"Hello {request.Name}!");

app.Run();

// This will force the compiler to generate a public class instead of an internal one
public partial class Program
{
}