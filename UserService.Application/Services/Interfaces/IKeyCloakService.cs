using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.Services.Interfaces
{
    public interface IKeyCloakService
    {
        Task<string> RegisterUserAsync(string email , string password ,string role , CancellationToken cancellationToken = default);
        Task<string> GetTokenAsync(string email , string password ,CancellationToken cancellationToken = default);
        Task DeleteUserAsync(string keycloakId, CancellationToken cancellationToken = default);
    }
}
