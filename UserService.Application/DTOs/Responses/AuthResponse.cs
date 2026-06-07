using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.DTOs.Responses
{
    public  record AuthResponse(
        string AccesToken,
        string TokenType,
        int ExpireIn);
    
}
