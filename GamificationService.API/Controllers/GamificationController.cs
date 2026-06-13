using GamificationService.Application.DTOs.Requests;
using GamificationService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GamificationService.API.Controllers
{
    [ApiController]
    [Route("api/v1/gamification")]
    [Authorize]
    public class GamificationController : ControllerBase
    {
        private readonly IGamificationService _gamificationService;

        public GamificationController(IGamificationService gamificationService)
        {
            _gamificationService = gamificationService;
        }

        private Guid GetUserID() =>
            Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException());

        [HttpGet("xp/me")]
        public async Task<IActionResult> GetMyXp(CancellationToken cancellationToken)
        {
            var result = await _gamificationService.GetUserXpAsync(GetUserID(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("xp/{userId}")]
        [Authorize(Policy = "ParentOrAdmin")]
        public async Task<IActionResult> GetUserXp(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _gamificationService.GetUserXpAsync(userId, cancellationToken);
            return Ok(result);
        }

        [HttpPost("xp/add")]
        [Authorize(Policy = "AdminOnly")]
       public async Task<IActionResult> AddXp(
       [FromBody] AddXpRequest request,
       CancellationToken cancellationToken)
        {
            var result = await _gamificationService.AddXpAsync(request, cancellationToken);
            return Ok(result);
        }


        [HttpGet("badges/me")]
        public async Task<IActionResult> GetMyBadges(CancellationToken cancellationToken)
        {
            var result = await _gamificationService.GetUserBadgesAsync(GetUserID(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("badges/{userId}")]
        [Authorize(Policy = "ParentOrAdmin")]
        public async Task<IActionResult> GetUserBadges(Guid userId, CancellationToken cancellationToken)
        {
            var result = await _gamificationService.GetUserBadgesAsync(userId, cancellationToken);
            return Ok(result);
        }

    }
}
