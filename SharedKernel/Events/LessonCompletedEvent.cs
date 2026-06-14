
namespace SharedKernel.Events;

public record LessonCompletedEvent(
    Guid UserId,
    Guid LessonId,
    Guid SessionId,
    int TotalXpEarned,
    string UserEmail,
    string DisplayName,
    DateTime CompletedAt
);