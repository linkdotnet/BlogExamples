using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

BenchmarkRunner.Run<SplitQueryBenchmarks>();

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.Net11_0)]
public class SplitQueryBenchmarks
{
    private SqliteConnection _connection = null!;
    private DbContextOptions<BloggingContext> _options = null!;

    [GlobalSetup]
    public void Setup()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<BloggingContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new BloggingContext(_options);
        context.Database.EnsureCreated();
        Seeder.Seed(context, blogCount: 5_000, postsPerBlog: 5);
    }

    [GlobalCleanup]
    public void Cleanup() => _connection.Dispose();

    [Benchmark]
    public List<Blog> SplitQuery()
    {
        using var context = new BloggingContext(_options);
        return BlogQuery(context).AsSplitQuery().ToList();
    }

    private static IQueryable<Blog> BlogQuery(BloggingContext context)
        => context.Blogs
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Include(b => b.Series)
            .Include(b => b.Posts);
}

public class BloggingContext(DbContextOptions<BloggingContext> options) : DbContext(options)
{
    public DbSet<Blog> Blogs => Set<Blog>();
}

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int SeriesId { get; set; }
    public Series Series { get; set; } = null!;

    public List<Post> Posts { get; set; } = [];
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
}

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Blog> Blogs { get; set; } = [];
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Blog> Blogs { get; set; } = [];
}

public class Series
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Blog> Blogs { get; set; } = [];
}

public static class Seeder
{
    public static void Seed(BloggingContext context, int blogCount, int postsPerBlog)
    {
        var random = new Random(42);

        var authors = Enumerable.Range(1, 500).Select(i => new Author { Name = $"Author {i}" }).ToArray();
        var categories = Enumerable.Range(1, 100).Select(i => new Category { Name = $"Category {i}" }).ToArray();
        var series = Enumerable.Range(1, 50).Select(i => new Series { Name = $"Series {i}" }).ToArray();

        context.AddRange(authors);
        context.AddRange(categories);
        context.AddRange(series);

        for (var i = 1; i <= blogCount; i++)
        {
            context.Add(new Blog
            {
                Name = $"Blog {i}",
                Author = authors[random.Next(authors.Length)],
                Category = categories[random.Next(categories.Length)],
                Series = series[random.Next(series.Length)],
                Posts = Enumerable.Range(1, postsPerBlog)
                    .Select(p => new Post { Title = $"Post {i}-{p}" })
                    .ToList(),
            });
        }

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}
