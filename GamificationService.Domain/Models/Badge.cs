using GamificationService.Domain.Enums;
using GamificationService.Domain.Exceptions;

namespace GamificationService.Domain.Models
{
    public class Badge
    {
        public Guid id {  get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string IconURL { get; private set; }
        public BadgeType Type { get; private set; }
        public int RequiredValue { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public ICollection<UserBadge> UserBadges { get; private set; } = new List<UserBadge>();


        private Badge() { }
        public static Badge Create(

            string name,
            string desc,
            string url,
            BadgeType type,
            int requiedvalue)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessException("Badge smund te jete pa emer.");

            if (requiedvalue <= 0)
                throw new BusinessException("Required value duhet me e madhe se 0.");

            return new Badge
            {
                id = Guid.NewGuid(),
                Name = name,
                Description = desc,
                IconURL = url,
                Type = type,
                RequiredValue = requiedvalue,
                CreatedAt = DateTime.UtcNow
            };
        }

            public void SoftDelete() => DeletedAt = DateTime.UtcNow;
        

    }
}
