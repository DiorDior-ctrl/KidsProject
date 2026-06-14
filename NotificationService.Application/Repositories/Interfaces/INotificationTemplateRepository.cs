
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Application.Repositories.Interfaces;

public interface INotificationTemplateRepository
{
    Task<NotificationTemplate?> GetByTypeAsync(NotificationType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationTemplate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(NotificationTemplate template, CancellationToken cancellationToken = default);
    Task DeleteAsync(NotificationTemplate template, CancellationToken cancellationToken = default);
}