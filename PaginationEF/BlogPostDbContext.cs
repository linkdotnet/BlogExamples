using Microsoft.EntityFrameworkCore;

namespace PaginationEF;

public class BlogPostDbContext : DbContext
{
    public DbSet<BlogPost> BlogPosts { get; set; }

    public BlogPostDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<BlogPost>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();
    }
}