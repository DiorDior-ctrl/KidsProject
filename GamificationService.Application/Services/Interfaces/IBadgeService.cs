
using GamificationService.Application.DTOs.Requests;
using GamificationService.Application.DTOs.Responses;

namespace GamificationService.Application.Services.Interfaces;

public interface IBadgeService
{
    Task<IEnumerable<BadgeResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BadgeResponse> CreateAsync(CreateBadgeRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}