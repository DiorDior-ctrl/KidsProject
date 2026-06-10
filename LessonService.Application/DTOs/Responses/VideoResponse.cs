
namespace LessonService.Application.DTOs.Responses;

public record VideoResponse(
    Guid Id,
    string StreamingUrl,
    int DurationSeconds,
    string Status
);