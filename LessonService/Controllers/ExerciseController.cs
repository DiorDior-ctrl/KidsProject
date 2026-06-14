
using LessonService.Application.DTOs.Requests;
using LessonService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace LessonService.API.Controllers;

[ApiController]
[Route("api/v1/exercises")]
[Authorize]
[EnableRateLimiting("GeneralPolicy")]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpGet("lesson/{lessonId}")]
    public async Task<IActionResult> GetByLessonId(Guid lessonId, CancellationToken cancellationToken)
    {
        var result = await _exerciseService.GetByLessonIdAsync(lessonId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _exerciseService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    // Endpoint vetëm për komunikim internal mes service-ve
    [HttpGet("{id}/answer")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetExerciseAnswer(Guid id, CancellationToken cancellationToken)
    {
        var result = await _exerciseService.GetByIdAsync(id, cancellationToken);
        return Ok(new { correctAnswer = result.CorrectAnswer, xpReward = result.XpReward });
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(
        [FromBody] CreateExerciseRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _exerciseService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _exerciseService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}