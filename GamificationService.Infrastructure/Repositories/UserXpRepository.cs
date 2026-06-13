using GamificationService.Application.Repositories.Interfaces;
using GamificationService.Domain.Models;
using GamificationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GamificationService.Infrastructure.Repositories
{
    public class UserXpRepository : IUserXpRepository
    {
        protected readonly GamificationServiceDbContext _context;

        public UserXpRepository(GamificationServiceDbContext context)
        {
            _context = context;
        }

        public async Task<UserXp?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserXps.FirstOrDefaultAsync(ux => ux.UserId == userId , cancellationToken);
        }
        public async Task AddAsync(UserXp userXp, CancellationToken cancellationToken = default)
        {
            await _context.UserXps.AddAsync(userXp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(UserXp userXp, CancellationToken cancellationToken = default)
        {
            _context.UserXps.Update(userXp);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
