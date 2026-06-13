
using GamificationService.Domain.Enums;
using GamificationService.Domain.Models;

namespace GamificationService.Application.Repositories.Interfaces;

public interface ILeaderboardRepository
{
    Task<IEnumerable<LeaderboardEntry>> GetByPeriodAsync(LeaderboardPeriod period, int top, CancellationToken cancellationToken = default);
    Task AddAsync(LeaderboardEntry entry, CancellationToken cancellationToken = default);
    Task UpdateAsync(LeaderboardEntry entry, CancellationToken cancellationToken = default);
}