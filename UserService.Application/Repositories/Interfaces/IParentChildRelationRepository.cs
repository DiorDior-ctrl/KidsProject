// UserService.Application/Repositories/Interfaces/IParentChildRelationRepository.cs
using UserService.Domain.Models;

namespace UserService.Application.Repositories.Interfaces;

public interface IParentChildRelationRepository
{
    Task<ParentChildRelation?> GetByParentAndChildAsync(Guid parentId, Guid childId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ParentChildRelation>> GetChildrenByParentIdAsync(Guid parentId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid parentId, Guid childId, CancellationToken cancellationToken = default);
    Task AddAsync(ParentChildRelation relation, CancellationToken cancellationToken = default);
    Task DeleteAsync(ParentChildRelation relation, CancellationToken cancellationToken = default);
}