using GamificationService.Application.DTOs.Requests;
using GamificationService.Application.Services.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.Events;

namespace GamificationService.Infrastructure.Messaging;

public class LessonCompletedConsumer : IConsumer<LessonCompletedEvent>
{
    private readonly IGamificationService _gamificationService;
    private readonly ILogger<LessonCompletedConsumer> _logger;

    public LessonCompletedConsumer(
        IGamificationService gamificationService,
        ILogger<LessonCompletedConsumer> logger)
    {
        _gamificationService = gamificationService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<LessonCompletedEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation("LessonCompleted event u mor për userin {UserId}", @event.UserId);

        await _gamificationService.AddXpAsync(new AddXpRequest(
            @event.UserId,
            @event.TotalXpEarned), context.CancellationToken);
    }
}