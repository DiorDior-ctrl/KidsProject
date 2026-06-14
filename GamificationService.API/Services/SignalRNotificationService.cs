using GamificationService.Application.Services.Interfaces;
using GamificationService.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GamificationService.API.Services;

public class SignalRNotificationService : IRealtimeNotificationService
{
    private readonly IHubContext<GamificationHub> _hubContext;
    private readonly ILogger<SignalRNotificationService> _logger;

    public SignalRNotificationService(
        IHubContext<GamificationHub> hubContext,
        ILogger<SignalRNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendXpUpdateAsync(
        Guid userId, int totalXp, int xpEarned,
        CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("XpUpdated", new
            {
                totalXp,
                xpEarned,
                timestamp = DateTime.UtcNow
            }, cancellationToken);

        _logger.LogInformation("XP update u dërgua real-time te useri {UserId}: +{Xp}", userId, xpEarned);
    }

    public async Task SendBadgeEarnedAsync(
        Guid userId, string badgeName, string iconUrl,
        CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("BadgeEarned", new
            {
                badgeName,
                iconUrl,
                timestamp = DateTime.UtcNow
            }, cancellationToken);

        _logger.LogInformation("Badge notification u dërgua real-time te useri {UserId}: {Badge}", userId, badgeName);
    }

    public async Task SendStreakUpdateAsync(
        Guid userId, int currentStreak,
        CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group($"user-{userId}")
            .SendAsync("StreakUpdated", new
            {
                currentStreak,
                timestamp = DateTime.UtcNow
            }, cancellationToken);
    }
}