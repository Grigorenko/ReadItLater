using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReadItLater.Data
{
    internal class RefConfiguration : IEntityTypeConfiguration<Ref>
    {
        public void Configure(EntityTypeBuilder<Ref> builder)
        {
            builder.HasIndex(p => p.Id).IsUnique();
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Url).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Image).IsRequired().HasMaxLength(500);

            builder.ToTable("Refs");
        }
    }
}
