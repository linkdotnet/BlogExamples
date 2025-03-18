using Microsoft.EntityFrameworkCore;

public class BloggingContext : DbContext
{
    public DbSet<BlogPost> BlogPosts { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("DataSource=myshareddb;mode=memory;cache=shared");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BlogPostTypeConfiguration());
    }
}