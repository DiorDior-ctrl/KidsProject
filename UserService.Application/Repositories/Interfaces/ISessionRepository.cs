// UserService.Application/Repositories/Interfaces/ISessionRepository.cs
using UserService.Domain.Models;

namespace UserService.Application.Repositories.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Session>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Session session, CancellationToken cancellationToken = default);
    Task UpdateAsync(Session session, CancellationToken cancellationToken = default);
    Task DeleteAsync(Session session, CancellationToken cancellationToken = default);
}