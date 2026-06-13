using GamificationService.Application.DTOs.Requests;
using GamificationService.Application.DTOs.Responses;
using GamificationService.Application.Repositories.Interfaces;
using GamificationService.Application.Services.Interfaces;
using GamificationService.Domain.Exceptions;
using GamificationService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace GamificationService.Application.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly ILogger<BadgeService> _logger;

        public BadgeService(IBadgeRepository badgeRepository , ILogger<BadgeService> logger)
        {
            _badgeRepository = badgeRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<BadgeResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var badges = await _badgeRepository.GetAllAsync(cancellationToken);

            return badges.Select(b => new BadgeResponse(
                b.id, b.Name, b.Description, b.IconURL, b.Type, b.RequiredValue));
        }
        public async Task<BadgeResponse> CreateAsync(CreateBadgeRequest request, CancellationToken cancellationToken = default)
        {
            var badge = Badge.Create(
                request.Name,
                request.Description,
                request.IconUrl,
                request.Type,
                request.RequiredValue);

            await _badgeRepository.AddAsync(badge, cancellationToken);
            _logger.LogInformation("Badge u krijua: {Name}", badge.Name);

            return new BadgeResponse(badge.id, badge.Name, badge.Description,
                badge.IconURL, badge.Type, badge.RequiredValue);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var badge = await _badgeRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Badge", id);



            await _badgeRepository.DeleteAsync(badge, cancellationToken);
            _logger.LogInformation("Badge u fshi: {id}", id);
        }
    }
}
