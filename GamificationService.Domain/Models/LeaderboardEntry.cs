using GamificationService.Domain.Enums;

namespace GamificationService.Domain.Models
{
    public class LeaderboardEntry
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string DisplayName { get; private set; }
        public int XpGained { get; private set; }
        public int Rank { get; private set; }
        public LeaderboardPeriod Period { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private LeaderboardEntry()
        {
            
        }

        public static LeaderboardEntry Create(
            Guid userid,
            string name,
            int xps,
            
            LeaderboardPeriod period)
        {
            return new LeaderboardEntry
            {
                Id = Guid.NewGuid(),
                UserId = userid,
                DisplayName = name,
                XpGained = xps,
                Rank = 0,
                Period = period,
                CreatedAt = DateTime.UtcNow
            };

        }
        public void SetRank(int rank) => Rank = rank;

    }
}
