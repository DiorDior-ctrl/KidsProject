using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.DTOs.Responses
{
    public record  UserResponse(
        Guid Id,
        string Email,
        string Role,
        bool IsActive,
        DateTime CreatedAt);
    
   
}
