using Microsoft.EntityFrameworkCore;

public class BlogContext : DbContext
{
    public DbSet<BlogPost> BlogPosts { get; set; }

    public BlogContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BlogPostConfiguration());
    }
}