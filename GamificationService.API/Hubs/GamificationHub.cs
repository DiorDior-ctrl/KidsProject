using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GamificationService.API.Hubs;

[Authorize]
public class GamificationHub : Hub
{
    private readonly ILogger<GamificationHub> _logger;

    public GamificationHub(ILogger<GamificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId != null)
        {
            // Shto userin në grupin e tij personal
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            _logger.LogInformation("User {UserId} u lidh me GamificationHub", userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId != null)
        {
            _logger.LogInformation("User {UserId} u shkëput nga GamificationHub", userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}