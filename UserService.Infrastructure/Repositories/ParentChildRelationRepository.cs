// UserService.Infrastructure/Repositories/ParentChildRelationRepository.cs
using Microsoft.EntityFrameworkCore;
using UserService.Application.Repositories.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories;

public class ParentChildRelationRepository : IParentChildRelationRepository
{
    private readonly UserServiceDbContext _context;

    public ParentChildRelationRepository(UserServiceDbContext context)
    {
        _context = context;
    }
    public async Task<ParentChildRelation?> GetByParentAndChildAsync(Guid parentId , Guid childId ,CancellationToken cancellationToken =default)
    {
        return await _context.ParentChildRelations
            .FirstOrDefaultAsync(r => r.ParentId == parentId && r.ChildID == childId , cancellationToken);
    }
    public async Task<IEnumerable<ParentChildRelation>> GetChildrenByParentIdAsync(Guid parentId ,CancellationToken cancellationToken = default)
    {
        return await _context.ParentChildRelations
            .Where( p => p.ParentId == parentId).ToListAsync(cancellationToken);
    }
    public async Task<bool> ExistsAsync(Guid parentId , Guid childId ,CancellationToken cancellationToken = default)
    {
        return await _context.ParentChildRelations
            .AnyAsync(r => r.ParentId == parentId && r.ChildID == childId, cancellationToken);
    }
    public async Task AddAsync(ParentChildRelation relation , CancellationToken cancellationToken = default)
    {
        await _context.ParentChildRelations.AddAsync(relation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(ParentChildRelation relation , CancellationToken cancellation = default)
    {
        _context.ParentChildRelations.Remove(relation);
        await _context.SaveChangesAsync(cancellation);
    }
}