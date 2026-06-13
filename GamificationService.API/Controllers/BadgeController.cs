
using GamificationService.Application.DTOs.Requests;
using GamificationService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamificationService.API.Controllers;

[ApiController]
[Route("api/v1/badges")]
[Authorize]
public class BadgeController : ControllerBase
{
    private readonly IBadgeService _badgeService;

    public BadgeController(IBadgeService badgeService)
    {
        _badgeService = badgeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _badgeService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(
        [FromBody] CreateBadgeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _badgeService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _badgeService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}