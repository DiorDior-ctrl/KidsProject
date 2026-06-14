
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;

namespace NotificationService.Application.Services.Interfaces;

public interface INotificationTemplateService
{
    Task<IEnumerable<NotificationTemplateResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<NotificationTemplateResponse> CreateAsync(CreateTemplateRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}