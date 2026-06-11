using Microsoft.EntityFrameworkCore;
using ProgressService.Application.Repositories.Interfaces;
using ProgressService.Domain.Models;
using ProgressService.Infrastructure.Data;

namespace ProgressService.Infrastructure.Repositories
{
    public class LessonSessionRepository : ILessonSessionRepository
    {
        protected readonly ProgressServiceDbContext _context;
        public LessonSessionRepository(ProgressServiceDbContext context)
        {
            _context = context;
        }

        public async Task<LessonSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.LessonSessions.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
        public async Task<LessonSession?> GetByIdWithAttemptsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.LessonSessions
                .Include(s => s.Attempts).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
                
        }
        public async Task<IEnumerable<LessonSession>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.LessonSessions
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.StartedAt)
                .ToListAsync(cancellationToken);
        }
        public async Task<LessonSession?> GetActiveSessionAsync(Guid userId, Guid lessonId, CancellationToken cancellationToken = default)
        {
            return await _context.LessonSessions
                .Where(s => s.UserId == userId
                && s.LessonId == lessonId && s.CompletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task AddAsync(LessonSession session, CancellationToken cancellationToken = default)
        {
            await _context.LessonSessions.AddAsync(session, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(LessonSession session, CancellationToken cancellationToken = default)
        {
             _context.LessonSessions.Update(session);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
