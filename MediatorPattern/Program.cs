// See https://aka.ms/new-console-template for more information

using System.Drawing;
using MediatorPattern;
using MediatorPattern.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Legend:");

ColorConsole.WriteLineCommandHandler("Command - Handlers are blue");
ColorConsole.WriteLineQueryHandler("Query - Handlers are yellow");
ColorConsole.WriteLineProgram("Program code is green");
ColorConsole.WriteLineRepository("Repository code is magenta");
Console.WriteLine("------------------------");
Console.WriteLine();

var serviceProvider = CreateServiceProvider();
await serviceProvider.GetRequiredService<Execution>().RunAsync();

ServiceProvider CreateServiceProvider()
{
    var collection = new ServiceCollection();
    // This will execute our main logic
    collection.AddScoped<Execution>();
    
    // This comes from the MediatR package.
    // It looks for all commands, queries and handlers and registers
    // them in the container
    collection.AddMediatR(typeof(Program).Assembly);
    
    // Our repository
    collection.AddScoped<UserRepository>();
    return collection.BuildServiceProvider();
}