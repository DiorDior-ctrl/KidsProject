
using LessonService.Domain.Models;

namespace LessonService.Application.Repositories.Interfaces;

public interface IExerciseRepository
{
    Task<Exercise?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Exercise>> GetByLessonIdAsync(Guid lessonId, CancellationToken cancellationToken = default);
    Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default);
    Task UpdateAsync(Exercise exercise, CancellationToken cancellationToken = default);
    Task DeleteAsync(Exercise exercise, CancellationToken cancellationToken = default);
}