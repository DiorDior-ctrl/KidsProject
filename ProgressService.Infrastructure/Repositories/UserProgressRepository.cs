using Microsoft.EntityFrameworkCore;
using ProgressService.Application.Repositories.Interfaces;
using ProgressService.Domain.Models;
using ProgressService.Infrastructure.Data;

namespace ProgressService.Infrastructure.Repositories
{
    public class UserProgressRepository : IUserProgressRepository
    {
        protected readonly ProgressServiceDbContext _context;

        public UserProgressRepository( ProgressServiceDbContext context)
        {
            _context = context;
        }

        public async Task<UserProgress?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserProgresses
                .FirstOrDefaultAsync(u => u.UserId == userId , cancellationToken);
        }
        public async Task AddAsync(UserProgress progress, CancellationToken cancellationToken = default)
        {
            await _context.UserProgresses.AddAsync(progress, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(UserProgress progress, CancellationToken cancellationToken = default)
        {
            _context.UserProgresses.Update(progress);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
