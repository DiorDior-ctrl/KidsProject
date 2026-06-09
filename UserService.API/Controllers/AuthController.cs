using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs.Requests;
using UserService.Application.Services.Interfaces;
namespace UserService.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register/parent")]
    public async Task<IActionResult> RegisterParent([FromBody] RegisterParentRequest request , CancellationToken cancellationToken)
    {
        var result = await _userService.RegisterParentAsync(request, cancellationToken);
        return Ok(result);
    }
    [HttpPost("register/child")]
    public async Task<IActionResult> RegisterChild([FromBody] RegisterChildRequest request ,CancellationToken cancellationToken)
    {

        var keycloakId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
        ?? throw new UnauthorizedAccessException();

        var parent = await _userService.GetByKeycloakIdAsync(keycloakId, cancellationToken);

        var result = await _userService.RegisterChildAsync(parent.Id, request, cancellationToken);
        return CreatedAtAction(nameof(RegisterChild), result);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request , CancellationToken cancellationToken)
    {
        var result = await _userService.LoginAsync(request, cancellationToken);
        return Ok(result);
    }

}

