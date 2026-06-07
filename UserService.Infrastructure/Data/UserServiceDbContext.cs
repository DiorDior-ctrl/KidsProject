using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Models;
namespace UserService.Infrastructure.Data
{
    public class UserServiceDbContext : DbContext
    {
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<ChildProfile> ChildProfiles => Set<ChildProfile>();
        public DbSet<ParentChildRelation> ParentChildRelations => Set<ParentChildRelation>();
        public DbSet<Session> Sessions => Set<Session>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserServiceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        //Audit trails
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        { 
            foreach(var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["DeletedAt"] = DateTime.UtcNow;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);  
        }
        
      
    }
}
