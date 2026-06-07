using System;
using System.Collections.Generic;
using System.Text;
using UserService.Application.DTOs.Requests;
using UserService.Application.DTOs.Responses;

namespace UserService.Application.Services.Interfaces
{
    public  interface IChildProfileService
    {
        Task<ChildProfileResponse> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<ChildProfileResponse> UpdateProfileAsync(Guid userId, UpdateChildProfileRequest request, CancellationToken cancellationToken = default);
    }
}
