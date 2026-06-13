
using GamificationService.Domain.Models;

namespace GamificationService.Application.Repositories.Interfaces;

public interface IUserXpRepository
{
    Task<UserXp?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(UserXp userXp, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserXp userXp, CancellationToken cancellationToken = default);
}