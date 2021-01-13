using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReadItLater.Data
{
    internal class TagRelConfiguration : IEntityTypeConfiguration<TagRef>
    {
        public void Configure(EntityTypeBuilder<TagRef> builder)
        {
            builder.HasIndex(p => new { p.RefId, p.TagId }).IsUnique();
            builder.HasKey(p => new { p.RefId, p.TagId });
            builder.HasOne(p => p.Ref).WithMany(p => p.TagRels).HasForeignKey(p => p.RefId);
            builder.HasOne(p => p.Tag).WithMany(p => p.TagRels).HasForeignKey(p => p.TagId);

            builder.ToTable("TagRefs");
        }
    }
}
