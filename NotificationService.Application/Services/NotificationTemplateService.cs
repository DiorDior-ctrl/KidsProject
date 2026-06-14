using Microsoft.Extensions.Logging;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;
using NotificationService.Application.Repositories.Interfaces;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Services
{
    public class NotificationTemplateService : INotificationTemplateService
    {
        private readonly INotificationTemplateRepository _templateRepository;
        private readonly ILogger<NotificationTemplateService> _logger;

        public NotificationTemplateService(
            INotificationTemplateRepository templateRepository,
            ILogger<NotificationTemplateService> logger)
        {
            _templateRepository = templateRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<NotificationTemplateResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var notifications = await _templateRepository.GetAllAsync(cancellationToken);
            return notifications.Select(t => new NotificationTemplateResponse(
            t.Id, t.Type, t.Subject, t.BodyTemplate, t.CreatedAt));
        }
        public async Task<NotificationTemplateResponse> CreateAsync(CreateTemplateRequest request, CancellationToken cancellationToken = default)
        {
            var template = NotificationTemplate.Create(
                request.Type,
                request.Subject,
                request.BodyTemplate);

            await _templateRepository.AddAsync(template, cancellationToken);
            _logger.LogInformation("Template u krijuar per tipin : {Type}", request.Type);

            return new NotificationTemplateResponse(
            template.Id, template.Type, template.Subject,
            template.BodyTemplate, template.CreatedAt);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var template = await _templateRepository.GetByTypeAsync(
                (await _templateRepository.GetAllAsync(cancellationToken))
                .First(t => t.Id == id).Type, cancellationToken)
                ?? throw new NotFoundException("Template", id);

            await _templateRepository.DeleteAsync(template, cancellationToken);
            _logger.LogInformation("Template u fshi: {Id}", id);
        }
    }
}
