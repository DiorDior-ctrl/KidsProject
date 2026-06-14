
namespace SharedKernel.Events;

public record BadgeEarnedEvent(
    Guid UserId,
    string UserEmail,
    string BadgeName,
    string BadgeIconUrl,
    DateTime EarnedAt
);