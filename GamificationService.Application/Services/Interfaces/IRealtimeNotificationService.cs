namespace GamificationService.Application.Services.Interfaces;

public interface IRealtimeNotificationService
{
    Task SendXpUpdateAsync(Guid userId, int totalXp, int xpEarned, CancellationToken cancellationToken = default);
    Task SendBadgeEarnedAsync(Guid userId, string badgeName, string iconUrl, CancellationToken cancellationToken = default);
    Task SendStreakUpdateAsync(Guid userId, int currentStreak, CancellationToken cancellationToken = default);
}