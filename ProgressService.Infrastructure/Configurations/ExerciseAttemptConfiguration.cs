
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgressService.Domain.Models;

namespace ProgressService.Infrastructure.Configurations;

public class ExerciseAttemptConfiguration : IEntityTypeConfiguration<ExerciseAttempt>
{
    public void Configure(EntityTypeBuilder<ExerciseAttempt> builder)
    {
        builder.ToTable("exercise_attempts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(a => a.LessonSessionId)
            .HasColumnName("lesson_session_id")
            .IsRequired();

        builder.Property(a => a.ExerciseId)
            .HasColumnName("exercise_id")
            .IsRequired();

        builder.Property(a => a.AnswerGiven)
            .HasColumnName("answer_given")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(a => a.IsCorrect)
            .HasColumnName("is_correct")
            .IsRequired();

        builder.Property(a => a.XpEarned)
            .HasColumnName("xp_earned")
            .HasDefaultValue(0);

        builder.Property(a => a.TimeTakenMs)
            .HasColumnName("time_taken_ms")
            .IsRequired();

        builder.Property(a => a.AttemptedAt)
            .HasColumnName("attempted_at")
            .IsRequired();

        builder.HasIndex(a => a.LessonSessionId)
            .HasDatabaseName("ix_exercise_attempts_session");
    }
}