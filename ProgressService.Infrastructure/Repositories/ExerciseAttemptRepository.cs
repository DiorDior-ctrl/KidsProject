using Microsoft.EntityFrameworkCore;
using ProgressService.Application.Repositories.Interfaces;
using ProgressService.Domain.Models;
using ProgressService.Infrastructure.Data;

namespace ProgressService.Infrastructure.Repositories
{
    public class ExerciseAttemptRepository : IExerciseAttemptRepository   
    {
        protected readonly ProgressServiceDbContext _context;

        public ExerciseAttemptRepository(ProgressServiceDbContext context)
        {
            _context = context;   
        }

        public async Task<IEnumerable<ExerciseAttempt>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            return await _context.ExerciseAttempts
                .Where(e => e.LessonSessionId == sessionId).OrderBy(e => e.AttemptedAt)
                .ToListAsync(cancellationToken);      
        }
        public async Task<int> CountBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            return await _context.ExerciseAttempts
                .CountAsync(e => e.LessonSessionId == sessionId, cancellationToken);
        }
        public async Task AddAsync(ExerciseAttempt attempt, CancellationToken cancellationToken = default)
        {
            await _context.ExerciseAttempts.AddAsync(attempt);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
