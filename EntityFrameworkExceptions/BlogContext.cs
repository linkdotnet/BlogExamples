using EntityFramework.Exceptions.Sqlite;
using Microsoft.EntityFrameworkCore;

public class BlogContext : DbContext
{
    public DbSet<BlogPost> BlogPosts { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite("Filename=:memory:")
            .UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>().Property(b => b.Title)
            .IsRequired();
    }
}