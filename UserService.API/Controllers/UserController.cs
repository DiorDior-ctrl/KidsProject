using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Application.DTOs.Requests;
using UserService.Application.Services.Interfaces;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IChildProfileService _childProfileService;

    public UserController(IUserService userService, IChildProfileService childProfileService)
    {
        _userService = userService;
        _childProfileService = childProfileService;
    }

    private string GetKeycloakId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException();

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var result = await _userService.GetByKeycloakIdAsync(
            GetKeycloakId(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("me/children")]
    [Authorize(Policy = "ParentOnly")]
    public async Task<IActionResult> GetMyChildren(CancellationToken cancellationToken)
    {
        var user = await _userService.GetByKeycloakIdAsync(
            GetKeycloakId(), cancellationToken);

        var result = await _userService.GetChildrenByParentIdAsync(
            user.Id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("me/children/link")]
    [Authorize(Policy = "ParentOnly")]
    public async Task<IActionResult> LinkChild(
        [FromBody] LinkChildRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByKeycloakIdAsync(
            GetKeycloakId(), cancellationToken);

        await _userService.LinkChildToParentAsync(user.Id, request, cancellationToken);
        return NoContent();
    }

    [HttpGet("{userId}/profile")]
    [Authorize(Policy = "ParentOrAdmin")]
    public async Task<IActionResult> GetChildProfile(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _childProfileService.GetByUserIdAsync(
            userId, cancellationToken);
        return Ok(result);
    }

    [HttpPut("me/profile")]
    [Authorize(Policy = "ChildOnly")]
    public async Task<IActionResult> UpdateMyProfile(
        [FromBody] UpdateChildProfileRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByKeycloakIdAsync(
            GetKeycloakId(), cancellationToken);

        var result = await _childProfileService.UpdateProfileAsync(
            user.Id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{userId}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeactivateUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        await _userService.DeactivateUserAsync(userId, cancellationToken);
        return NoContent();
    }
}