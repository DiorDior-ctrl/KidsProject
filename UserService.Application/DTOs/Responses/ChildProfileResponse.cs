using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.DTOs.Responses
{
    public record ChildProfileResponse(
        Guid Id,
        Guid UserId,
        string DisplayName,
        int Age,
        string AvatarId,
        int CurrentLevel);
   
}
