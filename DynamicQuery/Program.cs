using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using var dbContext = new BloggingContext();
dbContext.Database.EnsureCreated();

Console.WriteLine("What column would you like to return?");
var column = Console.ReadLine();

var columnSelector = GenerateColumnSelector(column).Compile();
var query = dbContext.BlogPosts.Select(s => new ReturnColumn
{
    Id = s.Id,
    Column = columnSelector.Invoke(s)
});

var queryString = query.ToQueryString();
Console.WriteLine(queryString);

var queryWithDynamic = dbContext
    .BlogPosts
    .Select<ReturnColumn>($"new(Id, {column} AS Column)");

var queryStringWithDynamic = queryWithDynamic.ToQueryString();
Console.WriteLine(queryStringWithDynamic);

Expression<Func<BlogPost, string>> GenerateColumnSelector(string? column)
{
    return column switch
    {
        "Title" => s => s.Title,
        "Content" => s => s.Content,
        "Author" => s => s.Author,
        _ => throw new ArgumentException("Invalid column name")
    };
}

public class ReturnColumn
{
    public int Id { get; set; }
    public string Column { get; set; }
}

public class BlogPostTypeConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(2048).IsRequired();
        builder.Property(x => x.Content).IsRequired();
        builder.Property(x => x.Author).HasMaxLength(2048).IsRequired();
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.Updated).IsRequired();
        builder.Property(x => x.Tags).IsRequired();
    }
}