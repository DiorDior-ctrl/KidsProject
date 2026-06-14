
using LessonService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LessonService.Infrastructure.Data;

public class LessonServiceDbContext : DbContext
{
    public LessonServiceDbContext(DbContextOptions<LessonServiceDbContext> options)
        : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<LessonVideo> LessonVideos => Set<LessonVideo>();
    public DbSet<Exercise> Exercises => Set<Exercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LessonServiceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.CurrentValues["DeletedAt"] = DateTime.UtcNow;
                entry.CurrentValues["IsActive"] = false;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}