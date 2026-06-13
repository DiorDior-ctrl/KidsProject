
using GamificationService.Domain.Models;

namespace GamificationService.Application.Repositories.Interfaces;

public interface IBadgeRepository
{
    Task<Badge?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Badge>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Badge badge, CancellationToken cancellationToken = default);
    Task DeleteAsync(Badge badge, CancellationToken cancellationToken = default);
}