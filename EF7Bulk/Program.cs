using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EF7Bulk;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

BenchmarkRunner.Run<UpdateBenchmark>();

[MemoryDiagnoser]
public class UpdateBenchmark
{
    private PeopleContext _peopleContext;

    [Params(100, 1_000)] public int RowsToUpdate { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _peopleContext = CreateContext();
        CreateContext();
        FillWithData();
    }

    [Benchmark(Baseline = true)]
    public async Task OneByOneUpdate()
    {
        var entries = await _peopleContext.People.ToListAsync();
        foreach (var entry in entries)
        {
            entry.Age = 21;
        }

        await _peopleContext.SaveChangesAsync();
    }

    [Benchmark]
    public async Task BulkUpdate()
    {
        await _peopleContext.People
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Age, p => 21));
    }

    private void FillWithData()
    {
        using var context = CreateContext();
        context.Database.EnsureCreated();
        for (var i = 1; i <= RowsToUpdate; i++)
            context.People.Add(new Person(i, "Steven", 31));

        context.SaveChanges();
    }

    private static PeopleContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=myshareddb;mode=memory;cache=shared");
        connection.Open();
        var options = new DbContextOptionsBuilder()
            .UseSqlite(connection)
            .Options;
        return new PeopleContext(options);
    }
}