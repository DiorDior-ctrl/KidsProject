using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

public class ChildProfileConfiguration : IEntityTypeConfiguration<ChildProfile>
{
    public void Configure(EntityTypeBuilder<ChildProfile> builder)
    {
        builder.ToTable("child_profiles");

        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(cp => cp.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(cp => cp.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(cp => cp.Age)
            .HasColumnName("age")
            .IsRequired();

        builder.Property(cp => cp.AvatarId)
            .HasColumnName("avatar_id")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(cp => cp.CurrentLevel)
            .HasColumnName("current_level")
            .HasDefaultValue(1);

        builder.Property(cp => cp.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.Property(cp => cp.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.HasQueryFilter(cp => cp.DeletedAt == null);

        builder.HasIndex(cp => cp.UserId)
            .HasDatabaseName("ix_child_profiles_user_id");
    }
}