// See https://aka.ms/new-console-template for more information

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PaginationEF;

var connection = CreateInMemoryConnection();

var options = new DbContextOptionsBuilder()
    .UseSqlite(CreateInMemoryConnection())
    .Options;

var db = new BlogPostDbContext(options);
await AddInstancesAsync(20, db);

var page1 = await db.BlogPosts.ToPagedListAsync(1, 15);

Console.WriteLine("Count: " + page1.Count);

connection.Close();

SqliteConnection CreateInMemoryConnection()
{
    var sqliteConnection = new SqliteConnection("DataSource=:memory:");
    sqliteConnection.Open();
    return sqliteConnection;
}

async Task AddInstancesAsync(int instanceCount, BlogPostDbContext context)
{
    for (var i = 0; i < instanceCount; i++)
    {
        await context.BlogPosts.AddAsync(new BlogPost() { Title = "Hello " + i });
    }

    await context.SaveChangesAsync();
}