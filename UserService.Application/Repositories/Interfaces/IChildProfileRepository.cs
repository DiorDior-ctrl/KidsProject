// UserService.Application/Repositories/Interfaces/IChildProfileRepository.cs
using UserService.Domain.Models;

namespace UserService.Application.Repositories.Interfaces;

public interface IChildProfileRepository
{
    Task<ChildProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChildProfile>> GetByParentIdAsync(Guid parentId, CancellationToken cancellationToken = default);
    Task AddAsync(ChildProfile childProfile, CancellationToken cancellationToken = default);
    Task UpdateAsync(ChildProfile childProfile, CancellationToken cancellationToken = default);
    Task DeleteAsync(ChildProfile childProfile, CancellationToken cancellationToken = default);
}