using System;
using System.Collections.Generic;
using System.Text;
using UserService.Domain.Exceptions;
namespace UserService.Domain.Models
{
    public class Session
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string DeviceInfo { get; private set; } = string.Empty;
        public DateTime LastActivity { get; private set; } 
        public DateTime CreatedAt { get; private set; } 
        public DateTime? DeletedAt { get; private set; }

        public User User { get; private set; } = null!;
        private Session()
        {
             
        }

        public static Session Created(Guid userId ,string deviceInfo)
        {
            if (string.IsNullOrWhiteSpace(deviceInfo))
                throw new BusinessException("Device info smund te jete bosh!");

            return new Session
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DeviceInfo = deviceInfo,
                LastActivity = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
        }
        public void UpdateLastActivity()
        {
            LastActivity = DateTime.UtcNow;
        }
        public void SoftDelete() 
        {
            DeletedAt = DateTime.UtcNow;
        }
    }
}
