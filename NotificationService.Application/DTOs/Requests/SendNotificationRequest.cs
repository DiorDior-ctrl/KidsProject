
using NotificationService.Domain.Enums;

namespace NotificationService.Application.DTOs.Requests;

public record SendNotificationRequest(
    Guid UserId,
    string RecipientEmail,
    NotificationType Type,
    Dictionary<string, string> Variables
);