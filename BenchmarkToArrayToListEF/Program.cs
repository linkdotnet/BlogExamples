using System.Data.Common;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

[MemoryDiagnoser]
public class ToArrayVsToListBenchmark  
{  
    private BlogContext? _context;
    private DbConnection? _connection;

    [GlobalSetup(Targets = [nameof(ToArrayAsyncBenchmark), nameof(ToListAsyncBenchmark)])]  
    public void Setup()  
    {
        _connection = CreateInMemoryConnection();
        var options = new DbContextOptionsBuilder()
            .UseSqlite(_connection)
            .Options;
        _context = new BlogContext(options);  
        _context.Database.EnsureDeleted();  
        _context.Database.EnsureCreated();  

        DataSeeder.Seed(_context, 10000);
    }  

    [Params(100, 1000, 10000)]  
    public int NumberOfElements { get; set; }  

    [Benchmark]  
    public async Task<List<BlogPost>> ToListAsyncBenchmark()  
    {  
        return await _context!.BlogPosts.Take(NumberOfElements).ToListAsync();  
    }  

    [Benchmark]  
    public async Task<BlogPost[]> ToArrayAsyncBenchmark()  
    {  
        return await _context!.BlogPosts.Take(NumberOfElements).ToArrayAsync();  
    }  

    [GlobalCleanup]  
    public void Cleanup()  
    {  
        _context?.Dispose();
        _connection?.Dispose();
    }

    public static void Main() => BenchmarkRunner.Run<ToArrayVsToListBenchmark>();
    
    private static SqliteConnection CreateInMemoryConnection()
    {
        var connection = new SqliteConnection(string.Empty);

        connection.Open();

        return connection;
    }
}