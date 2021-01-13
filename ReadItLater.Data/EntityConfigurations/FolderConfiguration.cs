using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReadItLater.Data
{
    internal class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.HasIndex(p => p.Id).IsUnique();
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.HasMany(p => p.Refs).WithOne(p => p.Folder).HasForeignKey(p => p.FolderId);
            builder.HasOne(p => p.Parent).WithMany(p => p.Folders).HasForeignKey(p => p.ParentId).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Folders");
        }
    }
}
