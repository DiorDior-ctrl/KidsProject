using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.DTOs.Requests
{
    public record LoginRequest(
        string Email,
        string Password);
}
