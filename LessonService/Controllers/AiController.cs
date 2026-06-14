using LessonService.Application.DTOs.Requests;
using LessonService.Application.Services.Interfaces;
using LessonService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LessonService.API.Controllers;

[ApiController]
[Route("api/v1/ai")]
[Authorize(Policy = "AdminOnly")]
[EnableRateLimiting("GeneralPolicy")]
public class AiController : ControllerBase
{
    private readonly IAiExerciseService _aiExerciseService;
    private readonly ILogger<AiController> _logger;

    public AiController(IAiExerciseService aiExerciseService, ILogger<AiController> logger)
    {
        _aiExerciseService = aiExerciseService;
        _logger = logger;
    }

    [HttpPost("generate-exercises")]
    public async Task<IActionResult> GenerateExercises(
        [FromBody] GenerateExercisesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _aiExerciseService.GenerateExercisesAsync(
            request.LessonId,
            request.LessonTitle,
            request.Topic,
            request.Count,
            request.Type,
            cancellationToken);

        return Ok(result);
    }
}