
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LessonService.Application.DTOs.Requests;
using LessonService.Application.Services.Interfaces;

namespace LessonService.API.Controllers;

[ApiController]
[Route("api/v1/lessons")]
[Authorize]
public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet("module/{moduleId}")]
    public async Task<IActionResult> GetByModuleId(Guid moduleId, CancellationToken cancellationToken)
    {
        var result = await _lessonService.GetByModuleIdAsync(moduleId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _lessonService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(
        [FromBody] CreateLessonRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _lessonService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _lessonService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}