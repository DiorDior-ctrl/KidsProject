
namespace ProgressService.Application.DTOs.Responses;

public record VideoProgressResponse(
    bool VideoCompleted,
    bool CanStartExercises,
    int CurrentSeconds
);