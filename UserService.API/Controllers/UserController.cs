using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public UserController(IUserService userService , IChildProfileService childProfileService)
    {
       _userService = userService;
       _childProfileService = childProfileService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value
            ?? throw new UnauthorizedAccessException());

        var result = await _userService.GetByIdAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("me/children")]
    [Authorize(Policy = "ParentOnly")]
    public async Task<IActionResult> GetMyChildren(CancellationToken cancellation)
    {
        var userid = Guid.Parse(User.FindFirst("sub")?.Value
            ?? throw new UnauthorizedAccessException());

        var result = await _userService.GetChildrenByParentIdAsync(userid, cancellation);
        return Ok(result);
    }
    [HttpPost("me/children/link")]
    [Authorize(Policy = "ParentOnly")]
    public async Task<IActionResult> LinkChild(
        [FromBody] LinkChildRequest request,
        CancellationToken cancellationToken)
    {
        var parentId = Guid.Parse(User.FindFirst("sub")?.Value  ?? throw new UnauthorizedAccessException());

         await _userService.LinkChildToParentAsync(parentId, request, cancellationToken);
        return NoContent();
    }
    [HttpGet("{userId}/profile")]
    [Authorize(Policy = "ParentOrAdmin")]
    public async Task<IActionResult> GetChildProfile(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var child = await _childProfileService.GetByUserIdAsync(userId, cancellationToken);
        return Ok(child);
    }
    [HttpPut("me/profile")]
    [Authorize(Policy = "ChildOnly")]
    public async Task<IActionResult> UpdateMyProfile(
        [FromBody] UpdateChildProfileRequest request,
        CancellationToken cancellationToken)
    {
        var childId = Guid.Parse(User.FindFirst("sub")?.Value
            ??  throw new UnauthorizedAccessException());

        var result = await _childProfileService.UpdateProfileAsync(childId, request, cancellationToken);
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