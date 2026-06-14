using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;
namespace NotificationService.API.Controllers
{
    [ApiController]
    [Route("api/v1/notifications")]
    [Authorize]
    [EnableRateLimiting("GeneralPolicy")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notification)
        {
            _notificationService = notification;
        }

        private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException());

        [HttpGet("me")]
        public async Task<IActionResult> GetMyNotifications(CancellationToken cancellationToken)
        {
            var result = await _notificationService.GetByUserIdAsync(GetUserId(), cancellationToken);
            return Ok(result);
        }

        [HttpPost("send")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Send(
        [FromBody] SendNotificationRequest request,
        CancellationToken cancellationToken)
        {
            var resutl = await _notificationService.SendAsync(request, cancellationToken);
            return Ok(resutl);
        }

        [HttpPost("process-pending")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> ProcessPending(CancellationToken cancellationToken)
        {
            await _notificationService.ProcessPendingAsync(cancellationToken);
            return Ok(new { message = "Pending notifications u procesuan."});
        }
    }
}
