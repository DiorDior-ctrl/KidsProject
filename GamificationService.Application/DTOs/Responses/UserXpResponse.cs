
namespace GamificationService.Application.DTOs.Responses;

public record UserXpResponse(
    Guid UserId,
    int TotalXp,
    int CurrentStreak,
    int LongestStreak,
    DateTime? LastActivityDate
);