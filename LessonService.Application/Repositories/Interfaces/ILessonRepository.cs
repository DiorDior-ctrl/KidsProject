
using LessonService.Domain.Models;

namespace LessonService.Application.Repositories.Interfaces;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Lesson?> GetByIdWithExercisesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Lesson>> GetByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken = default);
    Task AddAsync(Lesson lesson, CancellationToken cancellationToken = default);
    Task UpdateAsync(Lesson lesson, CancellationToken cancellationToken = default);
    Task DeleteAsync(Lesson lesson, CancellationToken cancellationToken = default);
}