
namespace GamificationService.Application.DTOs.Responses;

public record UserBadgeResponse(
    Guid BadgeId,
    string BadgeName,
    string IconUrl,
    DateTime EarnedAt
);