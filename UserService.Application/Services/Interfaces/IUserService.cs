using System;
using System.Collections.Generic;
using System.Text;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;
namespace UserService.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> RegisterParentAsync(RegisterParentRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> RegisterChildAsync(Guid parentId, RegisterChildRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<UserResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserResponse> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken = default);
        Task LinkChildToParentAsync(Guid parentId, LinkChildRequest request, CancellationToken cancellationToken = default);
        Task<IEnumerable<ChildProfileResponse>> GetChildrenByParentIdAsync(Guid parentId, CancellationToken cancellationToken = default);
        Task DeactivateUserAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
