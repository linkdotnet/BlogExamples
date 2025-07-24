using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

[MemoryDiagnoser]
public class EntityFrameworkBenchmark
{
    private const int Iterations = 1000;
    private TestDbContextWithThreadSafety? _dbContextWithThreadSafety;
    private TestDbContextWithoutThreadSafety? _dbContextWithoutThreadSafety;

    [GlobalSetup]
    public void Setup()
    {
        _dbContextWithThreadSafety = new TestDbContextWithThreadSafety();
        _dbContextWithThreadSafety.Database.EnsureCreated();
        
        _dbContextWithoutThreadSafety = new TestDbContextWithoutThreadSafety();
        _dbContextWithoutThreadSafety.Database.EnsureCreated();
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        if (_dbContextWithThreadSafety is not null)
        {
            await _dbContextWithThreadSafety.DisposeAsync();
        }
        
        if (_dbContextWithoutThreadSafety is not null)
        {
            await _dbContextWithoutThreadSafety.DisposeAsync();
        }
    }

    [Benchmark(Baseline = true)]
    public async Task<List<TestEntity>> WithThreadSafetyChecks()
    {
        var results = new List<TestEntity>();
        for (var i = 0; i < Iterations; i++)
        {
            results.AddRange(await _dbContextWithThreadSafety!.TestEntities.ToListAsync());
        }

        return results;
    }

    [Benchmark]
    public async Task<List<TestEntity>> WithoutThreadSafetyChecks()
    {
        var results = new List<TestEntity>();
        for (var i = 0; i < Iterations; i++)
        {
            results.AddRange(await _dbContextWithoutThreadSafety!.TestEntities.ToListAsync());
        }

        return results;
    }
}