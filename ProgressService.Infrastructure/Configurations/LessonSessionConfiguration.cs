
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgressService.Domain.Models;

namespace ProgressService.Infrastructure.Configurations;

public class LessonSessionConfiguration : IEntityTypeConfiguration<LessonSession>
{
    public void Configure(EntityTypeBuilder<LessonSession> builder)
    {
        builder.ToTable("lesson_sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(s => s.LessonId)
            .HasColumnName("lesson_id")
            .IsRequired();

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(s => s.VideoProgressSeconds)
            .HasColumnName("video_progress_seconds")
            .HasDefaultValue(0);

        builder.Property(s => s.VideoCompleted)
            .HasColumnName("video_completed")
            .HasDefaultValue(false);

        builder.Property(s => s.TotalXpEarned)
            .HasColumnName("total_xp_earned")
            .HasDefaultValue(0);

        builder.Property(s => s.StartedAt)
            .HasColumnName("started_at")
            .IsRequired();

        builder.Property(s => s.CompletedAt)
            .HasColumnName("completed_at")
            .IsRequired(false);

        builder.HasIndex(s => new { s.UserId, s.LessonId })
            .HasDatabaseName("ix_lesson_sessions_user_lesson");

        builder.HasMany(s => s.Attempts)
            .WithOne(a => a.LessonSession)
            .HasForeignKey(a => a.LessonSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}