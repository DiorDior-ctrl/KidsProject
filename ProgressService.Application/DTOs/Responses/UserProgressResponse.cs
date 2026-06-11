
namespace ProgressService.Application.DTOs.Responses;

public record UserProgressResponse(
    Guid UserId,
    int TotalXp,
    int CurrentStreak,
    int LongestStreak,
    DateTime? LastActivityDate
);