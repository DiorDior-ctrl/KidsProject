
using ProgressService.Domain.Models;

namespace ProgressService.Application.Repositories.Interfaces;

public interface IUserProgressRepository
{
    Task<UserProgress?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(UserProgress progress, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserProgress progress, CancellationToken cancellationToken = default);
}