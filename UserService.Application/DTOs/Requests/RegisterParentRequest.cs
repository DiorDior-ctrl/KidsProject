using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.DTOs.Requests
{
    public record RegisterParentRequest(string Email , string Password , string ConfirmPassword);
}
