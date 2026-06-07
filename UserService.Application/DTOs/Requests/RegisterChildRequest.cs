using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.DTOs.Requests
{
    public  record RegisterChildRequest(
        string Email,
        string Password,
        string DisplayName,
        int Age,
        string AvatarId);
}
