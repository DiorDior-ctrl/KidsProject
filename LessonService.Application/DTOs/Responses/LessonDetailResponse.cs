// LessonDetailResponse.cs
namespace LessonService.Application.DTOs.Responses;

public record LessonDetailResponse(
    Guid Id,
    Guid ModuleId,
    string Title,
    int OrderIndex,
    int XpReward,
    bool HasVideo,
    VideoResponse? Video,
    IEnumerable<ExerciseResponse> Exercises,
    DateTime CreatedAt
);