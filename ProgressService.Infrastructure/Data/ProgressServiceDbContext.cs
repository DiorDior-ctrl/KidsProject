using Microsoft.EntityFrameworkCore;
using ProgressService.Domain.Models;

namespace ProgressService.Infrastructure.Data
{
    public class ProgressServiceDbContext : DbContext
    {
        public ProgressServiceDbContext(DbContextOptions<ProgressServiceDbContext> options) : base(options)
        {
            
        }
        public DbSet<LessonSession> LessonSessions => Set<LessonSession>();
        public DbSet<ExerciseAttempt> ExerciseAttempts => Set<ExerciseAttempt>();
        public DbSet<UserProgress> UserProgresses => Set<UserProgress>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ProgressServiceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(
       CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
