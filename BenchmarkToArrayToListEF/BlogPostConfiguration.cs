using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.Property(b => b.Title)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(b => b.Subtitle)
            .HasMaxLength(4000)
            .IsRequired();
    }
}