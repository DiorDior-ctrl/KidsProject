
namespace ProgressService.Application.DTOs.Responses;

public record LessonSessionResponse(
    Guid SessionId,
    Guid LessonId,
    string Status,
    bool VideoCompleted,
    int VideoProgressSeconds,
    int TotalXpEarned,
    DateTime StartedAt
);