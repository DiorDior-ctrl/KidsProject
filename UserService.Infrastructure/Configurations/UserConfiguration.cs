using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;
using UserService.Domain.Enums;
namespace UserService.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) 
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            builder.Property(x => x.KeycloakId).HasColumnName("keycloak_id").HasMaxLength(256).IsRequired();
            builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(256).IsRequired();
            builder.Property(x => x.Role).HasColumnName("role").HasConversion<String>().IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(x => x.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            builder.Property(x => x.DeletedAt).HasColumnName("deleted_at").IsRequired(false);

            //Soft delete filter
            builder.HasQueryFilter(u => u.DeletedAt == null);

            //Indexes on email and keycloakid
            builder.HasIndex(x => x.Email).IsUnique().HasDatabaseName("ix_user_email");
            builder.HasIndex(x => x.KeycloakId).IsUnique().HasDatabaseName("ix_user_keycloak");

            //Relationships
            builder.HasOne(x => x.ChildProfile)
                .WithOne(y => y.User)
                .HasForeignKey<ChildProfile>(y => y.UserId)
                .OnDelete(DeleteBehavior.Cascade);
           
            builder.HasMany(x => x.Session)
                .WithOne(y => y.User)
                .HasForeignKey(y => y.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
