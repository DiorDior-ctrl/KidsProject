
using GamificationService.Domain.Enums;

namespace GamificationService.Application.DTOs.Responses;

public record LeaderboardEntryResponse(
    Guid UserId,
    string DisplayName,
    int XpGained,
    int Rank,
    LeaderboardPeriod Period
);