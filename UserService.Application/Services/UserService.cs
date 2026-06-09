using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
using UserService.Application.Repositories.Interfaces;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Exceptions;
using UserService.Domain.Models;
namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IChildProfileRepository _childProfileRepository;
        private readonly IParentChildRelationRepository _relationRepository;
        private readonly IKeyCloakService _keycloakService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository , IChildProfileRepository childProfileRepository , IParentChildRelationRepository relationRepository
            , IKeyCloakService keycloakService , ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _childProfileRepository = childProfileRepository;   
            _relationRepository = relationRepository;   
            _keycloakService = keycloakService;
            _logger = logger;   

        }

        public async Task<AuthResponse> RegisterParentAsync( RegisterParentRequest request ,CancellationToken cancellationToken = default)
        {
            var exists = await _userRepository.ExistsByEmailAsync(request.Email,cancellationToken);
            if (exists)
                throw new BusinessException("Ky email eshte regjistruar tashme");

            var keycloakId = await _keycloakService.RegisterUserAsync(request.Email, request.Password, "Parent", cancellationToken);
            var user = User.CreateParent(keycloakId, request.Email);
            await _userRepository.AddAsync(user , cancellationToken);

            _logger.LogInformation("Parent u regjistrua:{Email}", request.Email);

            var token = await _keycloakService.GetTokenAsync(request.Email , request.Password , cancellationToken);
            return new AuthResponse(token, "Bearer", 900);
            
        }
        public async Task<AuthResponse> RegisterChildAsync(Guid parentId, RegisterChildRequest request, CancellationToken cancellationToken = default)
        {
            var parent = await _userRepository.GetByIdAsync(parentId, cancellationToken)
                ?? throw new NotFoundException("User",parentId);

            var exist = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if(exist is not null)
                throw new BusinessException("Ky femije ekziston!");

            var keycloakId = await _keycloakService
                .RegisterUserAsync(request.Email, request.Password, "Child", cancellationToken);

            var child = User.CreateChild(keycloakId, request.Email);
            await _userRepository.AddAsync (child , cancellationToken);

            var profile = ChildProfile.Create(
                child.Id, request.DisplayName,  request.Age, request.AvatarId);
            await _childProfileRepository.AddAsync (profile , cancellationToken);

            var relation = ParentChildRelation.Create(parentId, child.Id);
            await _relationRepository.AddAsync(relation , cancellationToken);

            _logger.LogInformation("Femija u regjistrua: {Email} nga prindi {ParentId}", request.Email, parentId);

            var token = await _keycloakService.GetTokenAsync(request.Email , request.Password , cancellationToken);

            return new AuthResponse(token, "Bearer", 900);
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email,cancellationToken)
                ?? throw new NotFoundException("Useri nuk u gjet!");

            if (!user.IsActive)
                throw new BusinessException("Kjo llogari u caktivizua!");

            var token = await _keycloakService.GetTokenAsync(request.Email , request.Password , cancellationToken);

            _logger.LogInformation("Useri hyri: {Email}",request.Email);
            return new AuthResponse(token, "Bearer", 900);
                
        }
        public async Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id,cancellationToken) 
                ?? throw new NotFoundException("User" ,id);

            return new UserResponse(
                user.Id,
                user.Email,
                user.Role.ToString(),
                user.IsActive,
                user.CreatedAt
                );
        }
        public async Task<UserResponse> GetByKeycloakIdAsync(
    string keycloakId,
    CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByKeycloakIdAsync(keycloakId, cancellationToken)
                ?? throw new NotFoundException("User nuk u gjet.");

            return new UserResponse(
                user.Id,
                user.Email,
                user.Role.ToString(),
                user.IsActive,
                user.CreatedAt);
        }
        public async Task LinkChildToParentAsync(Guid parentId, LinkChildRequest request, CancellationToken cancellationToken = default)
        {
            var parent = await _userRepository.GetByIdAsync(parentId, cancellationToken)
                ?? throw new NotFoundException("User", parentId);

            var child = await _userRepository.GetByIdAsync(request.ChildId, cancellationToken)
                ?? throw new NotFoundException("User", request.ChildId);

            var exist = await _relationRepository.ExistsAsync(parentId , request.ChildId , cancellationToken);
            if (exist)
                throw new BusinessException("Ky femije eshte i lidhur tashme!");

            _logger.LogInformation("Femija {ChildId} u lidh me prindin {ParentId}", request.ChildId, parentId);
        }
        public async Task<IEnumerable<ChildProfileResponse>> GetChildrenByParentIdAsync(Guid parentId, CancellationToken cancellationToken = default)
        {
            var profiles = await _childProfileRepository.GetByParentIdAsync(parentId, cancellationToken);

            return profiles.Select(p => new ChildProfileResponse(
                p.Id,
                p.UserId,
                p.DisplayName,
                p.Age,
                p.AvatarId,
                p.CurrentLevel));
        }
        public async Task DeactivateUserAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("User", id);

            user.SoftDelete();
            await _userRepository.UpdateAsync(user,cancellationToken);
            await _keycloakService.DeleteUserAsync(user.KeycloakId, cancellationToken);

            _logger.LogInformation("User u deaktivizua {Id}", id);
  
        }
    }
}
