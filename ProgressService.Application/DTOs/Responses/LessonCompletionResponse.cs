
namespace ProgressService.Application.DTOs.Responses;

public record LessonCompletionResponse(
    int TotalXpEarned,
    int CurrentStreak,
    int TotalXp
);