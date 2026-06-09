using Microsoft.EntityFrameworkCore;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Domain.Models;
using LessonService.Infrastructure.Data;

namespace LessonService.Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly LessonServiceDbContext _context;

        public LessonRepository(LessonServiceDbContext context)
        {
           _context = context;  
        }

        public async Task<Lesson?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Lessons.Include(l => l.Video).FirstOrDefaultAsync(l => l.Id == id , cancellationToken);
        }
        public async Task<Lesson?> GetByIdWithExercisesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Lessons.Include(l => l.Video)
                .Include(l => l.Exercises.OrderBy(e => e.OrderIndex))
                .FirstOrDefaultAsync(l => l.Id == id , cancellationToken);
        }

        public async Task<IEnumerable<Lesson>> GetByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken = default)
        {
            return await _context.Lessons.Where(m => m.ModuleId == moduleId)
                .OrderBy(m => m.Id).ToListAsync(cancellationToken);
        }
        public async Task AddAsync(Lesson lesson, CancellationToken cancellationToken = default)
        {
            await _context.Lessons.AddAsync(lesson, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Lesson lesson, CancellationToken cancellationToken = default)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync(cancellationToken);

        }
        public async Task DeleteAsync(Lesson lesson, CancellationToken cancellationToken = default)
        {
             _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
