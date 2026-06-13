
using GamificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamificationService.Infrastructure.Configurations;

public class LeaderboardEntryConfiguration : IEntityTypeConfiguration<LeaderboardEntry>
{
    public void Configure(EntityTypeBuilder<LeaderboardEntry> builder)
    {
        builder.ToTable("leaderboard_entries");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(l => l.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(l => l.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.XpGained)
            .HasColumnName("xp_gained")
            .HasDefaultValue(0);

        builder.Property(l => l.Rank)
            .HasColumnName("rank")
            .HasDefaultValue(0);

        builder.Property(l => l.Period)
            .HasColumnName("period")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasIndex(l => new { l.Period, l.Rank })
            .HasDatabaseName("ix_leaderboard_period_rank");
    }
}