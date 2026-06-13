using GamificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GamificationService.Infrastructure.Data
{
    public class GamificationServiceDbContext : DbContext
    {
        public GamificationServiceDbContext(DbContextOptions<GamificationServiceDbContext> options) : base(options) { }

        public DbSet<UserXp> UserXps => Set<UserXp>();
        public DbSet<Badge> Badges => Set<Badge>();
        public DbSet<UserBadge> UserBadges => Set<UserBadge>();
        public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(GamificationServiceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
