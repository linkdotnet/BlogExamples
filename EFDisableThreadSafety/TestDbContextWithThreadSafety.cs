using Microsoft.EntityFrameworkCore;

public class TestDbContextWithThreadSafety : DbContext
{
    private const string ConnectionString = "DataSource=file::memory:?cache=shared";
    public DbSet<TestEntity> TestEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite(ConnectionString)
            .EnableThreadSafetyChecks(true);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestEntity>().HasKey(e => e.Id);
        modelBuilder.Entity<TestEntity>().Property(e => e.Name).HasMaxLength(100).IsRequired();
    }
}