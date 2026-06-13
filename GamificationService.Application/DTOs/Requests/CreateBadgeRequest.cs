
using GamificationService.Domain.Enums;

namespace GamificationService.Application.DTOs.Requests;

public record CreateBadgeRequest(
    string Name,
    string Description,
    string IconUrl,
    BadgeType Type,
    int RequiredValue
);