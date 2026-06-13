using GamificationService.Application.Repositories.Interfaces;
using GamificationService.Domain.Enums;
using GamificationService.Domain.Models;
using GamificationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GamificationService.Infrastructure.Repositories
{
    public class LeaderboardRepository : ILeaderboardRepository
    {

        protected readonly GamificationServiceDbContext _context;

        public LeaderboardRepository(GamificationServiceDbContext context)
        {
         _context = context;   
        }

        public async Task<IEnumerable<LeaderboardEntry>> GetByPeriodAsync(LeaderboardPeriod period, int top, CancellationToken cancellationToken = default)
        {
            return await _context.LeaderboardEntries
                .Where(l => l.Period == period)
                .OrderBy(l => l.Rank)
                .Take(top)
                .ToListAsync(cancellationToken);
        }
        public async Task AddAsync(LeaderboardEntry entry, CancellationToken cancellationToken = default)
        {
            await _context.LeaderboardEntries.AddAsync(entry,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(LeaderboardEntry entry, CancellationToken cancellationToken = default)
        {
            _context.LeaderboardEntries.Update(entry);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
