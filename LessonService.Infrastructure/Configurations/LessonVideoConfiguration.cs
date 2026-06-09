
using LessonService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LessonService.Infrastructure.Configurations;

public class LessonVideoConfiguration : IEntityTypeConfiguration<LessonVideo>
{
    public void Configure(EntityTypeBuilder<LessonVideo> builder)
    {
        builder.ToTable("lesson_videos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(v => v.LessonId)
            .HasColumnName("lesson_id")
            .IsRequired();

        builder.Property(v => v.StorageKey)
            .HasColumnName("storage_key")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(v => v.StreamingUrl)
            .HasColumnName("streaming_url")
            .HasMaxLength(1000);

        builder.Property(v => v.DurationSeconds)
            .HasColumnName("duration_seconds");

        builder.Property(v => v.FileSizeBytes)
            .HasColumnName("file_size_bytes");

        builder.Property(v => v.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(v => v.UploadedAt)
            .HasColumnName("uploaded_at")
            .IsRequired();

        builder.Property(v => v.ProcessedAt)
            .HasColumnName("processed_at")
            .IsRequired(false);

        builder.Property(v => v.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.HasQueryFilter(v => v.DeletedAt == null);
    }
}