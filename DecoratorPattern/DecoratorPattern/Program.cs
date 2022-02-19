using System.Diagnostics;
using DecoratorPattern;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

var provider = new ServiceCollection()
    .AddMemoryCache()
    .AddScoped<SlowRepository>()
    .AddScoped<IRepository>(p =>

    {
        var memoryCache = p.GetRequiredService<IMemoryCache>();
        var repository = p.GetRequiredService<SlowRepository>();
        return new CachedRepository(memoryCache, repository);
    })
    .BuildServiceProvider();

var person = new Person(1, "Steven Giesel");
var repository = provider.GetRequiredService<IRepository>();
await repository.SavePersonAsync(person);

var stopwatch = Stopwatch.StartNew();
await repository.GetPersonById(1);
Console.WriteLine($"First call took {stopwatch.ElapsedMilliseconds} ms");

stopwatch.Restart();
await repository.GetPersonById(1);
Console.WriteLine($"Second call took {stopwatch.ElapsedMilliseconds} ms");