using NotificationService.Application.Services.Interfaces;

namespace NotificationService.API.Jobs;

public class ProcessPendingNotificationsJob
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<ProcessPendingNotificationsJob> _logger;

    public ProcessPendingNotificationsJob(
        INotificationService notificationService,
        ILogger<ProcessPendingNotificationsJob> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("ProcessPendingNotifications job filloi");
        await _notificationService.ProcessPendingAsync();
        _logger.LogInformation("ProcessPendingNotifications job mbaroi");
    }
}