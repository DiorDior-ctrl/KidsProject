
using GamificationService.Application.DTOs.Responses;
using GamificationService.Domain.Enums;

namespace GamificationService.Application.Services.Interfaces;

public interface ILeaderboardService
{
    Task<IEnumerable<LeaderboardEntryResponse>> GetLeaderboardAsync(LeaderboardPeriod period, int top, CancellationToken cancellationToken = default);
    Task UpdateLeaderboardAsync(Guid userId, string displayName, int xpGained, LeaderboardPeriod period, CancellationToken cancellationToken = default);
}