using GamificationService.Domain.Exceptions;

namespace GamificationService.Domain.Models
{
    public class UserXp
    {
        public Guid Id { get; private set; }
        public Guid UserId {  get; private set; }
        public int TotalXp { get; private set; }
        public int CurrentStreak { get; private set; }
        public int LongestStreak { get; private set; }
        public DateTime? LastActivityDate {  get; private set; }
        public DateTime CreatedAt {  get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private UserXp()
        {
             
        }

        public static UserXp Create(Guid userid)
        {
            return new UserXp
            {
                Id = Guid.NewGuid(),
                UserId = userid,
                TotalXp = 0,
                CurrentStreak = 0,
                LastActivityDate = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
        }

        public void AddXp(int xp)
        {
            if (xp <= 0)
                throw new BusinessException("Xp duhet te jete me i madh se 0");

            TotalXp += xp;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatedStreak()
        {
            var today = DateTime.UtcNow;

            if(LastActivityDate == null || LastActivityDate.Value.Date < today.AddDays(-1))
            {
                CurrentStreak = 1; 
            }
            else if(LastActivityDate.Value.Date == today.AddDays(-1))
            {
                CurrentStreak++;
            }
            
            if(CurrentStreak > LongestStreak)
            {
                LongestStreak = CurrentStreak;  
            }
            LastActivityDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
               
        }
    }
}
