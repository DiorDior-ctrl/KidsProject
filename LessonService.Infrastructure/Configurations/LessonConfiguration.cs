
using LessonService.Domain.Models;
using LessonService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessonService.Infrastructure.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("lessons");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(l => l.ModuleId)
            .HasColumnName("module_id")
            .IsRequired();

        builder.Property(l => l.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(l => l.OrderIndex)
            .HasColumnName("order_index")
            .IsRequired();

        builder.Property(l => l.XpReward)
            .HasColumnName("xp_reward")
            .IsRequired();

        builder.Property(l => l.HasVideo)
            .HasColumnName("has_video")
            .HasDefaultValue(false);

        builder.Property(l => l.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(l => l.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.Property(l => l.Embedding)
        .HasColumnName("embedding")
        .HasColumnType("vector(1536)")
        .IsRequired(false);

        builder.HasQueryFilter(l => l.DeletedAt == null);

        builder.HasIndex(l => new { l.ModuleId, l.OrderIndex })
            .HasDatabaseName("ix_lessons_module_order");

        builder.HasOne(l => l.Video)
            .WithOne(v => v.Lesson)
            .HasForeignKey<LessonVideo>(v => v.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.Exercises)
            .WithOne(e => e.Lesson)
            .HasForeignKey(e => e.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}