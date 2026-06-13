using GamificationService.Application.DTOs.Responses;
using GamificationService.Application.Repositories.Interfaces;
using GamificationService.Application.Services.Interfaces;
using GamificationService.Domain.Enums;
using GamificationService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace GamificationService.Application.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ILeaderboardRepository _leaderboardRepository;
        private readonly ILogger<LeaderboardService> _logger;

        public LeaderboardService(ILeaderboardRepository leaderboardRepository , ILogger<LeaderboardService> logger)
        {
            _leaderboardRepository = leaderboardRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<LeaderboardEntryResponse>> GetLeaderboardAsync(LeaderboardPeriod period, int top, CancellationToken cancellationToken = default)
        {
            var entries = await _leaderboardRepository.GetByPeriodAsync(period, top, cancellationToken);

            return entries.Select(l => new LeaderboardEntryResponse(
                l.UserId,
                l.DisplayName,
                l.XpGained,
                l.Rank,
                l.Period));
        }
        public async Task UpdateLeaderboardAsync(Guid userId, string displayName, int xpGained, LeaderboardPeriod period, CancellationToken cancellationToken = default)
        {
            var entry = LeaderboardEntry.Create(userId, displayName, xpGained, period);

            await _leaderboardRepository.AddAsync(entry, cancellationToken);

            _logger.LogInformation("Leaderboard u perditesua per userin {UserId}", userId);
        }
    }
}
