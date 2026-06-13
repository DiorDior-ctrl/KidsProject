
using GamificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamificationService.Infrastructure.Configurations;

public class UserBadgeConfiguration : IEntityTypeConfiguration<UserBadge>
{
    public void Configure(EntityTypeBuilder<UserBadge> builder)
    {
        builder.ToTable("user_badges");

        builder.HasKey(ub => ub.Id);

        builder.Property(ub => ub.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(ub => ub.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(ub => ub.BadgeId)
            .HasColumnName("badge_id")
            .IsRequired();

        builder.Property(ub => ub.EarnedAt)
            .HasColumnName("earned_at")
            .IsRequired();

        builder.HasIndex(ub => new { ub.UserId, ub.BadgeId })
            .IsUnique()
            .HasDatabaseName("ix_user_badges_user_badge");
    }
}