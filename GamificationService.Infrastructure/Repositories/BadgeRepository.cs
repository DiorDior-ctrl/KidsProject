using GamificationService.Application.Repositories.Interfaces;
using GamificationService.Domain.Models;
using GamificationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GamificationService.Infrastructure.Repositories
{
    public class BadgeRepository : IBadgeRepository
    {
        protected readonly GamificationServiceDbContext _context;

        public BadgeRepository(GamificationServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Badge?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Badges.FirstOrDefaultAsync(b => b.id == id , cancellationToken);
        }

        public async Task<IEnumerable<Badge>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Badges.OrderBy(b => b.Name).ToListAsync(cancellationToken);
        }
        public async Task AddAsync(Badge badge, CancellationToken cancellationToken = default)
        {
            await _context.Badges.AddAsync(badge, cancellationToken);   
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Badge badge, CancellationToken cancellationToken = default)
        {
            badge.SoftDelete();
            _context.Badges.Update(badge);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
