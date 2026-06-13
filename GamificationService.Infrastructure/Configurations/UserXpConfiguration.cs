
using GamificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamificationService.Infrastructure.Configurations;

public class UserXpConfiguration : IEntityTypeConfiguration<UserXp>
{
    public void Configure(EntityTypeBuilder<UserXp> builder)
    {
        builder.ToTable("user_xps");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(u => u.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(u => u.TotalXp)
            .HasColumnName("total_xp")
            .HasDefaultValue(0);

        builder.Property(u => u.CurrentStreak)
            .HasColumnName("current_streak")
            .HasDefaultValue(0);

        builder.Property(u => u.LongestStreak)
            .HasColumnName("longest_streak")
            .HasDefaultValue(0);

        builder.Property(u => u.LastActivityDate)
            .HasColumnName("last_activity_date")
            .IsRequired(false);

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(u => u.UserId)
            .IsUnique()
            .HasDatabaseName("ix_user_xps_user_id");
    }
}