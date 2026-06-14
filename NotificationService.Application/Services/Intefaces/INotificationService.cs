
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.DTOs.Responses;

namespace NotificationService.Application.Services.Interfaces;

public interface INotificationService
{
    Task<NotificationResponse> SendAsync(SendNotificationRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationResponse>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task ProcessPendingAsync(CancellationToken cancellationToken = default);
}