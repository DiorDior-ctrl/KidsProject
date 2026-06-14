
using NotificationService.Domain.Enums;

namespace NotificationService.Application.DTOs.Responses;

public record NotificationTemplateResponse(
    Guid Id,
    NotificationType Type,
    string Subject,
    string BodyTemplate,
    DateTime CreatedAt
);