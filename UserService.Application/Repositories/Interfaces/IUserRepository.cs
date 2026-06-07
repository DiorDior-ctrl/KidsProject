using System;
using System.Collections.Generic;
using System.Text;
using UserService.Domain.Models;
namespace UserService.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByKeycloakIdAsync(string keycloakid , CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email , CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(User user ,  CancellationToken cancellationToken = default);
    }
}
