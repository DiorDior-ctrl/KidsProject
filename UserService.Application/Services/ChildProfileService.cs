using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.Repositories.Interfaces;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Exceptions;
namespace UserService.Application.Services
{
    public class ChildProfileService : IChildProfileService
    {
        private readonly IChildProfileRepository _childProfileRepository;

        public ChildProfileService(IChildProfileRepository childProfileRepository)
        {
             _childProfileRepository = childProfileRepository;
        }
        public async Task<ChildProfileResponse> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _childProfileRepository.GetByUserIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException("ChildProfile", userId);

            return new ChildProfileResponse(
                user.Id,
                user.UserId,
                user.DisplayName,
                user.Age,
                user.AvatarId,
                user.CurrentLevel
            );
        }
        public async Task<ChildProfileResponse> UpdateProfileAsync(Guid userId, UpdateChildProfileRequest request, CancellationToken cancellationToken = default)
        {
            var child = await _childProfileRepository.GetByUserIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException("Child", userId);

            child.UpdateAvatar(request.AvatarId);
            await _childProfileRepository.UpdateAsync(child,cancellationToken);

            return new ChildProfileResponse(
                child.Id,
                child.UserId,
                child.DisplayName,
                child.Age,
                child.AvatarId,
                child.CurrentLevel
                );

        }
    }
}
