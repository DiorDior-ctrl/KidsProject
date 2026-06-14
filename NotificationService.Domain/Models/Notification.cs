using NotificationService.Domain.Enums;
using NotificationService.Domain.Exceptions;

namespace NotificationService.Domain.Models
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string RecipientEmail { get; private set; } = string.Empty;
        public NotificationType Type { get; private set; }
        public string Subject { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;
        public NotificationStatus Status { get; private set; }
        public string? ErrorMessage { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? SentAt { get; private set; }

        private Notification()
        {
            
        }

        public static Notification Create(
        Guid userId,
        string recipientEmail,
        NotificationType type,
        string subject,
        string body)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                throw new BusinessException("Email nuk mund te jete bosh!");

            if (string.IsNullOrWhiteSpace(subject))
                throw new BusinessException("Subject nuk mund te jete bosh!");

            return new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RecipientEmail = recipientEmail,
                Type = type,
                Subject = subject,
                Body = body,
                Status = NotificationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void MarkAsSent()
        {
            Status = NotificationStatus.Sent;
            SentAt = DateTime.UtcNow;
        }
        public void MarkAsFailed(string errorMessage)
        {
            Status = NotificationStatus.Failed;
            ErrorMessage = errorMessage;
        }
    }
}
