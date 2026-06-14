using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Domain.Enums;
using SharedKernel.Events;

namespace NotificationService.Infrastructure.Messaging;

public class LessonCompletedConsumer : IConsumer<LessonCompletedEvent>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<LessonCompletedConsumer> _logger;

    public LessonCompletedConsumer(
        INotificationService notificationService,
        ILogger<LessonCompletedConsumer> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<LessonCompletedEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation("LessonCompleted event u mor për email {Email}", @event.UserEmail);

        if (string.IsNullOrEmpty(@event.UserEmail)) return;

        await _notificationService.SendAsync(new SendNotificationRequest(
            @event.UserId,
            @event.UserEmail,
            NotificationType.LessonCompleted,
            new Dictionary<string, string>
            {
                ["ChildName"] = @event.DisplayName,
                ["XpEarned"] = @event.TotalXpEarned.ToString()
            }), context.CancellationToken);
    }
}