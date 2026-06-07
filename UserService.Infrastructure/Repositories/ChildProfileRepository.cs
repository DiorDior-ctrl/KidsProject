using System;
using System.Collections.Generic;
using System.Text;
using UserService.Domain.Models;
using UserService.Application.Repositories.Interfaces;
using UserService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace UserService.Infrastructure.Repositories
{
    public class ChildProfileRepository : IChildProfileRepository
    {
        private readonly UserServiceDbContext _context;

        public ChildProfileRepository(UserServiceDbContext context)
        {
            _context = context; 
        }

        public async Task<ChildProfile?> GetByUserIdAsync(Guid userId , CancellationToken cancellationToken = default)
        {
            return await _context.ChildProfiles
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        }
        public async Task<IEnumerable<ChildProfile>> GetByParentIdAsync(Guid parentId , CancellationToken cancellationToken = default)
        {
            return await _context.ChildProfiles
                .Where(cp => _context.ParentChildRelations.Any(r => r.ParentId == parentId && r.ChildID == cp.UserId))
                .ToListAsync(cancellationToken);
        }
        public async Task AddAsync(ChildProfile childProfile , CancellationToken cancellationToken = default)
        {
            await _context.ChildProfiles.AddAsync(childProfile , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(ChildProfile childProfile , CancellationToken cancellationToken = default)
        {
            _context.ChildProfiles.Update(childProfile);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(ChildProfile childProfile , CancellationToken cancellationToken = default)
        {
            _context.ChildProfiles.Remove(childProfile);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
