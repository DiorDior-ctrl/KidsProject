using Microsoft.EntityFrameworkCore;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Domain.Models;
using LessonService.Infrastructure.Data;

namespace LessonService.Infrastructure.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        protected readonly LessonServiceDbContext _context;

        public ExerciseRepository(LessonServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Exercise>> GetByLessonIdAsync(Guid lessonId, CancellationToken cancellationToken = default)
        {
            return await _context.Exercises.Where(e => e.LessonId == lessonId)
                .OrderBy(e => e.OrderIndex).ToListAsync(cancellationToken);
        }
        public async Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default)
        {
            await _context.Exercises.AddAsync(exercise, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Exercise exercise, CancellationToken cancellationToken = default)
        {
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async  Task DeleteAsync (Exercise exercise, CancellationToken cancellationToken = default)
        {
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync(cancellationToken) ;
        }
    }
}
