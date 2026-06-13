using GamificationService.Application.Repositories.Interfaces;
using GamificationService.Domain.Models;
using GamificationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GamificationService.Infrastructure.Repositories
{
    public class UserBadgeRepository : IUserBadgeRepository
    {
        protected readonly GamificationServiceDbContext _context;

        public UserBadgeRepository(GamificationServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserBadge>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserBadges
                .Include(u => u.Badge)
                .Where(u => u.UserId == userId)
                .OrderByDescending(u => u.EarnedAt)
                .ToListAsync(cancellationToken);
        }
        public async Task<bool> ExistsAsync(Guid userId, Guid badgeId, CancellationToken cancellationToken = default)
        {
            return await _context.UserBadges.AnyAsync(ub => ub.UserId == userId || ub.BadgeId == badgeId, cancellationToken);
        }
        public async Task AddAsync(UserBadge userBadge, CancellationToken cancellationToken = default)
        {
            await _context.UserBadges .AddAsync(userBadge, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
