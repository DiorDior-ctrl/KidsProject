using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UserService.Domain.Exceptions;

namespace UserService.Domain.Models
{
    public class ChildProfile
    {
        public Guid Id { get;  private set; }
        public Guid UserId { get; private set; }
        public string DisplayName { get; private set; } = string.Empty;
        public int Age { get; private set; }
        public string AvatarId { get; private set; } = string.Empty;
        public int CurrentLevel { get; private set; }

        public DateTime UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public User User { get; private set; } = null!;

        private ChildProfile() { }

        public static ChildProfile Create(Guid userid, string displayName, int age, string avatarId) 
        {
            if (age < 4 || age > 12)
                throw new BusinessException("Mosha duhet te jete ndermjet 4 dhe 12 vjec!");

            if (string.IsNullOrWhiteSpace(displayName))
                throw new BusinessException("Emri i shfaqur nuk mund te jete bosh!");

            return new ChildProfile
            {
                Id = Guid.NewGuid(),
                UserId = userid,
                DisplayName = displayName.Trim(),
                Age = age,
                AvatarId = avatarId,
                CurrentLevel = 1,
                UpdatedAt = DateTime.UtcNow

            };
        }
        public void UpdateLevel(int newLevel)
        {
            if (CurrentLevel >= newLevel)
                throw new BusinessException("Niveli i ri duhet te jete me i larte se ai aktual!");

            CurrentLevel = newLevel;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAvatar(string avatarId)
        {
            if (string.IsNullOrWhiteSpace(avatarId))
                throw new BusinessException("Avatar ID  nuk mund te jete bosh!");
            AvatarId = avatarId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete() 
        {
            DeletedAt = DateTime.UtcNow;
        }
    }
}
