
namespace GamificationService.Domain.Models
{
    public class UserBadge
    {
        public Guid id {  get; private set; }
        public Guid UserId {  get; private set; }
        public Guid BadgeId {  get; private set; }
        public DateTime EarnedAt { get; private set; }

        public Badge Badge { get; private set; } = null!;

        private UserBadge()
        {
            
        }

        public static UserBadge Create(Guid userid ,  Guid badgeid)
        {
            return new UserBadge
            {
                id = Guid.NewGuid(),
                UserId = userid,
                BadgeId = badgeid,
                EarnedAt = DateTime.UtcNow,
            };
        }
    }
}
