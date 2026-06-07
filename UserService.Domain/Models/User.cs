using System;
using System.Collections.Generic;
using System.Text;
using UserService.Domain.Enums;

namespace UserService.Domain.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string KeycloakId { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public ChildProfile? ChildProfile { get; private set; }
        public ICollection<Session> Session { get; private set; } = new List<Session>();

        private User() { }

        public static User CreateParent(string keycloakId, string email)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                KeycloakId = keycloakId,
                Email = email.ToLowerInvariant().Trim(),
                Role = UserRole.Parent,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
        public static User CreateChild(string keycloakId, string email)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                KeycloakId = keycloakId,
                Role = UserRole.Child,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }
        public void SoftDelete()
        { 
            IsActive = false;  
            DeletedAt = DateTime.UtcNow;
        }
        public void Activate()
        {
            IsActive = true;
            DeletedAt = null;
        }
    }
}
