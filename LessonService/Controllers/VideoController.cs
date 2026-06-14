using LessonService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LessonService.API.Controllers;

[ApiController]
[Route("api/v1/videos")]
[Authorize]
[EnableRateLimiting("GeneralPolicy")]
public class VideoController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly ILessonService _lessonService;
    private readonly ILogger<VideoController> _logger;

    private const string VideoBucket = "lesson-videos";
    private const long MaxVideoSizeBytes = 500 * 1024 * 1024; // 500MB

    public VideoController(
        IStorageService storageService,
        ILessonService lessonService,
        ILogger<VideoController> logger)
    {
        _storageService = storageService;
        _lessonService = lessonService;
        _logger = logger;
    }

    [HttpPost("{lessonId}/upload")]
    [Authorize(Policy = "AdminOnly")]
    [RequestSizeLimit(524288000)] // 500MB
    public async Task<IActionResult> UploadVideo(
        Guid lessonId,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        // Validim i file-it
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "File nuk mund të jetë bosh." });

        if (file.Length > MaxVideoSizeBytes)
            return BadRequest(new { message = "File është shumë i madh. Maksimumi 500MB." });

        var allowedTypes = new[] { "video/mp4", "video/avi", "video/mov", "video/wmv" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            return BadRequest(new { message = "Tipi i file-it nuk lejohet. Vetëm MP4, AVI, MOV, WMV." });

        // Gjenero emrin unik të file-it
        var extension = Path.GetExtension(file.FileName);
        var objectName = $"lessons/{lessonId}/{Guid.NewGuid()}{extension}";

        using var stream = file.OpenReadStream();

        var storageKey = await _storageService.UploadAsync(
            VideoBucket,
            objectName,
            stream,
            file.ContentType,
            cancellationToken);

        var presignedUrl = await _storageService.GetPresignedUrlAsync(
            VideoBucket,
            objectName,
            expirySeconds: 86400, // 24 orë
            cancellationToken);

        _logger.LogInformation("Video u ngarkua për leksionin {LessonId}: {ObjectName}",
            lessonId, objectName);

        return Ok(new
        {
            lessonId,
            storageKey,
            streamingUrl = presignedUrl,
            fileName = file.FileName,
            fileSize = file.Length,
            contentType = file.ContentType
        });
    }

    [HttpGet("{lessonId}/url")]
    public async Task<IActionResult> GetVideoUrl(
        Guid lessonId,
        [FromQuery] string storageKey,
        CancellationToken cancellationToken)
    {
        var url = await _storageService.GetPresignedUrlAsync(
            VideoBucket,
            storageKey,
            expirySeconds: 3600,
            cancellationToken);

        return Ok(new { url });
    }
}