using Microsoft.EntityFrameworkCore;

namespace EF7Bulk;

public class PeopleContext : DbContext
{
    public DbSet<Person> People { get; set; } = default!;

    public PeopleContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasKey(p => p.Id);
        modelBuilder.Entity<Person>().Property(p => p.Name)
            .HasMaxLength(1024)
            .IsUnicode(false);
    }
}