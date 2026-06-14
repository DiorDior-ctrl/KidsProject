
using GamificationService.Application.Services.Interfaces;
using GamificationService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace GamificationService.API.Controllers;

[ApiController]
[Route("api/v1/leaderboard")]
[Authorize]
[EnableRateLimiting("GeneralPolicy")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpGet("weekly")]
    public async Task<IActionResult> GetWeekly(
        [FromQuery] int top = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _leaderboardService.GetLeaderboardAsync(
            LeaderboardPeriod.Weekly, top, cancellationToken);
        return Ok(result);
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GetDaily(
        [FromQuery] int top = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _leaderboardService.GetLeaderboardAsync(
            LeaderboardPeriod.Daily, top, cancellationToken);
        return Ok(result);
    }

    [HttpGet("alltime")]
    public async Task<IActionResult> GetAllTime(
        [FromQuery] int top = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _leaderboardService.GetLeaderboardAsync(
            LeaderboardPeriod.AllTime, top, cancellationToken);
        return Ok(result);
    }
}