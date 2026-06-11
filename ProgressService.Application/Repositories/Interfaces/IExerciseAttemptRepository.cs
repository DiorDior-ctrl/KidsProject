
using ProgressService.Domain.Models;

namespace ProgressService.Application.Repositories.Interfaces;

public interface IExerciseAttemptRepository
{
    Task<IEnumerable<ExerciseAttempt>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<int> CountBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task AddAsync(ExerciseAttempt attempt, CancellationToken cancellationToken = default);
}