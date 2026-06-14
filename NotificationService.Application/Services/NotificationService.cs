using Microsoft.Extensions.Logging;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Application.Repositories.Interfaces;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationTemplateRepository _notificationTemplateRepository;
        private readonly IEmailService _emailService;
        private ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            INotificationTemplateRepository notificationTemplateRepository,
            IEmailService emailService,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _notificationTemplateRepository = notificationTemplateRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<NotificationResponse> SendAsync(SendNotificationRequest request, CancellationToken cancellationToken = default)
        {
            //Merr fillimisht nje template nga databaza
            var template = await _notificationTemplateRepository.GetByTypeAsync(request.Type, cancellationToken)
                ?? throw new NotFoundException($"Template per tipin {request.Type} nuk u gjet.");

            //Render body
            var body = template.RenderBody(request.Variables);

            //Krijo notificationin ne db
            var notification = Notification.Create(
                request.UserId,
                request.RecipientEmail,
                request.Type,
                template.Subject,
                body);

            await _notificationRepository.AddAsync(notification, cancellationToken);

            //Dergo emailin
            try
            {
                await _emailService.SendAsync(
                request.RecipientEmail,
                template.Subject,
                body,
                cancellationToken);

                notification.MarkAsSent();
                await _notificationRepository.UpdateAsync(notification, cancellationToken);
                _logger.LogInformation("Notification u dergua me sukses: {Email}", request.RecipientEmail);
            }
            catch(Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
                await _notificationRepository.UpdateAsync(notification, cancellationToken);

                _logger.LogError(ex, "Dergimi i email deshtoi te : {Email}", request.RecipientEmail);
            }
            return MapToResponse(notification);
        }
        public async Task<IEnumerable<NotificationResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(userId, cancellationToken);
            return notifications.Select(MapToResponse);
        }
        public async Task ProcessPendingAsync(CancellationToken cancellationToken = default)
        {
            var pending = await _notificationRepository.GetPendingAsync(cancellationToken);

            foreach(var notification in pending)
            {
                try
                {
                    await _emailService.SendAsync(
                    notification.RecipientEmail,
                    notification.Subject,
                    notification.Body,
                    cancellationToken);

                    notification.MarkAsSent();
                    await _notificationRepository.UpdateAsync(notification, cancellationToken);

                    _logger.LogInformation("Pending notification u dërgua: {Id}", notification.Id);
                }
                catch (Exception ex)
                {
                    notification.MarkAsFailed(ex.Message);
                    await _notificationRepository.UpdateAsync(notification, cancellationToken);

                    _logger.LogError(ex, "Pending notifikim dështoi: {Id}", notification.Id);
                }
            }
        }
        private static NotificationResponse MapToResponse(Notification notification)
        {
            return new NotificationResponse(
                notification.Id,
                notification.UserId,
                notification.RecipientEmail,
                notification.Type,
                notification.Subject,
                notification.Status,
                notification.CreatedAt,
                notification.SentAt);
        }
    }
}
