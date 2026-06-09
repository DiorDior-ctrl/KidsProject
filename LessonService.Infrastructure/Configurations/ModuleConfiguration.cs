
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LessonService.Domain.Models;

namespace LessonService.Infrastructure.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("modules");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(m => m.CourseId)
            .HasColumnName("course_id")
            .IsRequired();

        builder.Property(m => m.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.OrderIndex)
            .HasColumnName("order_index")
            .IsRequired();

        builder.Property(m => m.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(m => m.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.HasQueryFilter(m => m.DeletedAt == null);

        builder.HasIndex(m => new { m.CourseId, m.OrderIndex })
            .HasDatabaseName("ix_modules_course_order");

        builder.HasMany(m => m.Lessons)
            .WithOne(l => l.Module)
            .HasForeignKey(l => l.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}