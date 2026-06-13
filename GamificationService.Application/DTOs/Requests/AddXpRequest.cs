
namespace GamificationService.Application.DTOs.Requests;

public record AddXpRequest(
    Guid UserId,
    int Xp
);