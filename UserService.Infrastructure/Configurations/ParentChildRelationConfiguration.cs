using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

public class ParentChildRelationConfiguration : IEntityTypeConfiguration<ParentChildRelation>
{
    public void Configure(EntityTypeBuilder<ParentChildRelation> builder)
    {
        builder.ToTable("parent_child_relations");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(r => r.ParentId)
            .HasColumnName("parent_id")
            .IsRequired();

        builder.Property(r => r.ChildID)
            .HasColumnName("child_id")
            .IsRequired();

        builder.Property(r => r.LinkedAt)
            .HasColumnName("linked_at")
            .IsRequired();

        builder.Property(r => r.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.HasQueryFilter(r => r.DeletedAt == null);

        // Një prind nuk mund të lidhet dy herë me të njëjtin fëmijë
        builder.HasIndex(r => new { r.ParentId, r.ChildID })
            .IsUnique()
            .HasDatabaseName("ix_parent_child_unique");

        builder.HasOne(r => r.Parent)
            .WithMany()
            .HasForeignKey(r => r.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Child)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}