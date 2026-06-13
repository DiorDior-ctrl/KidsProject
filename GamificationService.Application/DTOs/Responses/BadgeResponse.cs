
using GamificationService.Domain.Enums;

namespace GamificationService.Application.DTOs.Responses;

public record BadgeResponse(
    Guid Id,
    string Name,
    string Description,
    string IconUrl,
    BadgeType Type,
    int RequiredValue
);