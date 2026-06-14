using NotificationService.Domain.Enums;
using NotificationService.Domain.Exceptions;

namespace NotificationService.Domain.Models
{
    public class NotificationTemplate
    {
        public Guid Id { get; private set; }
        public NotificationType Type { get; private set; }
        public string Subject { get; private set; } = string.Empty;
        public string BodyTemplate { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        private NotificationTemplate()
        {
            
        }

        public static NotificationTemplate Create(
        NotificationType type,
        string subject,
        string bodyTemplate)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new BusinessException("Subject nuk mund te jete bosh");

            if (string.IsNullOrWhiteSpace(bodyTemplate))
                throw new BusinessException("Body template nuk mund te jete bosh");

            return new NotificationTemplate
            {
                Id = Guid.NewGuid(),
                Type = type,
                Subject = subject,
                BodyTemplate = bodyTemplate,
                CreatedAt = DateTime.UtcNow
            };
        }

        public string RenderBody(Dictionary<string ,string> variables)
        {
            var body = BodyTemplate;
            foreach(var variable in variables)
            {
                body = body.Replace("${{{{{variable.Key}}}}}", variable.Value);
            }
            return body;
        }

        public void SoftDelete() => DeletedAt = DateTime.UtcNow;
    }
}
