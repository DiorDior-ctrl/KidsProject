using LessonService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LessonService.API.Controllers;

[ApiController]
[Route("api/v1/search")]
[Authorize]
[EnableRateLimiting("GeneralPolicy")]
public class SearchController : ControllerBase
{
    private readonly ISemanticSearchService _searchService;

    public SearchController(ISemanticSearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("lessons")]
    public async Task<IActionResult> SearchLessons(
        [FromQuery] string query,
        [FromQuery] int topK = 5,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Query nuk mund të jetë bosh." });

        var results = await _searchService.SearchAsync(query, topK, cancellationToken);
        return Ok(results);
    }

    [HttpPost("lessons/{lessonId}/index")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> IndexLesson(
        Guid lessonId,
        [FromQuery] string title,
        CancellationToken cancellationToken = default)
    {
        await _searchService.IndexLessonAsync(lessonId, title, cancellationToken);
        return Ok(new { message = "Leksioni u indeksua me sukses." });
    }
}