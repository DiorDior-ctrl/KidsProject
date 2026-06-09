using LessonService.Domain.Models;

namespace LessonService.Application.Repositories.Interfaces;

public interface ILessonVideoRepository
{
    Task<LessonVideo?> GetByLessonIdAsync(Guid lessonId, CancellationToken cancellationToken = default);
    Task AddAsync(LessonVideo video, CancellationToken cancellationToken = default);
    Task UpdateAsync(LessonVideo video, CancellationToken cancellationToken = default);
    Task DeleteAsync(LessonVideo video, CancellationToken cancellationToken = default);
}