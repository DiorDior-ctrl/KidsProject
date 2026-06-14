
using NotificationService.Domain.Enums;

namespace NotificationService.Application.DTOs.Responses;

public record NotificationResponse(
    Guid Id,
    Guid UserId,
    string RecipientEmail,
    NotificationType Type,
    string Subject,
    NotificationStatus Status,
    DateTime CreatedAt,
    DateTime? SentAt
);