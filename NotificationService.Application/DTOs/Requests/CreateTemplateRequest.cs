
using NotificationService.Domain.Enums;

namespace NotificationService.Application.DTOs.Requests;

public record CreateTemplateRequest(
    NotificationType Type,
    string Subject,
    string BodyTemplate
);