
using GamificationService.Domain.Models;

namespace GamificationService.Application.Repositories.Interfaces;

public interface IUserBadgeRepository
{
    Task<IEnumerable<UserBadge>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userId, Guid badgeId, CancellationToken cancellationToken = default);
    Task AddAsync(UserBadge userBadge, CancellationToken cancellationToken = default);
}