
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LessonService.Domain.Models;

namespace LessonService.Infrastructure.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("exercises");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.LessonId)
            .HasColumnName("lesson_id")
            .IsRequired();

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(e => e.OrderIndex)
            .HasColumnName("order_index")
            .IsRequired();

        builder.Property(e => e.ContentJson)
            .HasColumnName("content_json")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(e => e.CorrectAnswer)
            .HasColumnName("correct_answer")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.XpReward)
            .HasColumnName("xp_reward")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.HasQueryFilter(e => e.DeletedAt == null);

        builder.HasIndex(e => new { e.LessonId, e.OrderIndex })
            .HasDatabaseName("ix_exercises_lesson_order");
    }
}