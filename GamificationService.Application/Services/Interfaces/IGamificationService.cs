
using GamificationService.Application.DTOs.Requests;
using GamificationService.Application.DTOs.Responses;

namespace GamificationService.Application.Services.Interfaces;

public interface IGamificationService
{
    Task<UserXpResponse> GetUserXpAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserXpResponse> AddXpAsync(AddXpRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserBadgeResponse>> GetUserBadgesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task CheckAndAwardBadgesAsync(Guid userId, CancellationToken cancellationToken = default);
}