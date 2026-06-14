// GamificationService.cs
using GamificationService.Application.DTOs.Requests;
using GamificationService.Application.DTOs.Responses;
using GamificationService.Application.Repositories.Interfaces;
using GamificationService.Application.Services.Interfaces;
using GamificationService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace GamificationService.Application.Services;

public class GamificationService : IGamificationService
{
    private readonly IUserXpRepository _userXpRepository;
    private readonly IUserBadgeRepository _userBadgeRepository;
    private readonly IBadgeRepository _badgeRepository;
    private readonly ILogger<GamificationService> _logger;
    private readonly IRealtimeNotificationService _realtimeService;

    public GamificationService(
        IUserXpRepository userXpRepository,
        IUserBadgeRepository userBadgeRepository,
        IBadgeRepository badgeRepository,
        IRealtimeNotificationService realtimeService,
        ILogger<GamificationService> logger)
    {
        _userXpRepository = userXpRepository;
        _userBadgeRepository = userBadgeRepository;
        _badgeRepository = badgeRepository;
        _realtimeService = realtimeService;
        _logger = logger;
    }

    public async Task<UserXpResponse> GetUserXpAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userXp = await _userXpRepository.GetByUserIdAsync(userId, cancellationToken);

        if (userXp == null)
        {
            return new UserXpResponse(userId, 0, 0, 0, null);
        }

        return new UserXpResponse(
            userXp.UserId,
            userXp.TotalXp,
            userXp.CurrentStreak,
            userXp.LongestStreak,
            userXp.LastActivityDate);
    }

    public async Task<UserXpResponse> AddXpAsync(AddXpRequest request, CancellationToken cancellationToken = default)
    {
        var userXp = await _userXpRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (userXp == null)
        {
            userXp = UserXp.Create(request.UserId);
            userXp.AddXp(request.Xp);
            userXp.UpdatedStreak();
            await _userXpRepository.AddAsync(userXp, cancellationToken);
        }
        else
        {
            userXp.AddXp(request.Xp);
            userXp.UpdatedStreak();
            await _userXpRepository.UpdateAsync(userXp, cancellationToken);
            // Dërgo real-time notification
            await _realtimeService.SendXpUpdateAsync(
                request.UserId, userXp.TotalXp, request.Xp, cancellationToken);
        }

        _logger.LogInformation("XP u shtua për userin {UserId}: +{Xp}", request.UserId, request.Xp);

        await CheckAndAwardBadgesAsync(request.UserId, cancellationToken);

        return new UserXpResponse(
            userXp.UserId,
            userXp.TotalXp,
            userXp.CurrentStreak,
            userXp.LongestStreak,
            userXp.LastActivityDate);
    }

    public async Task<IEnumerable<UserBadgeResponse>> GetUserBadgesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userBadges = await _userBadgeRepository.GetByUserIdAsync(userId, cancellationToken);

        return userBadges.Select(ub => new UserBadgeResponse(
            ub.BadgeId,
            ub.Badge.Name,
            ub.Badge.IconURL,
            ub.EarnedAt));
    }

    public async Task CheckAndAwardBadgesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userXp = await _userXpRepository.GetByUserIdAsync(userId, cancellationToken);
        if (userXp == null) return;

        var allBadges = await _badgeRepository.GetAllAsync(cancellationToken);

        foreach (var badge in allBadges)
        {
            var alreadyEarned = await _userBadgeRepository.ExistsAsync(userId, badge.id, cancellationToken);
            if (alreadyEarned) continue;

            bool earned = badge.Type switch
            {
                Domain.Enums.BadgeType.XpMilestone => userXp.TotalXp >= badge.RequiredValue,
                Domain.Enums.BadgeType.StreakMilestone => userXp.CurrentStreak >= badge.RequiredValue,
                _ => false
            };

            if (earned)
            {
                var userBadge = UserBadge.Create(userId, badge.id);
                await _userBadgeRepository.AddAsync(userBadge, cancellationToken);
                await _realtimeService.SendBadgeEarnedAsync(
                    userId, badge.Name, badge.IconURL, cancellationToken);
                _logger.LogInformation("Badge u fitua: {BadgeName} nga useri {UserId}", badge.Name, userId);
            }
        }
    }
}