
using ProgressService.Domain.Models;

namespace ProgressService.Application.Repositories.Interfaces;

public interface ILessonSessionRepository
{
    Task<LessonSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LessonSession?> GetByIdWithAttemptsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LessonSession>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<LessonSession?> GetActiveSessionAsync(Guid userId, Guid lessonId, CancellationToken cancellationToken = default);
    Task AddAsync(LessonSession session, CancellationToken cancellationToken = default);
    Task UpdateAsync(LessonSession session, CancellationToken cancellationToken = default);
}