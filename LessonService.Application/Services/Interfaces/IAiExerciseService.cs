using LessonService.Application.DTOs.Responses;
using LessonService.Domain.Enums;

namespace LessonService.Application.Services.Interfaces;

public interface IAiExerciseService
{
    Task<IEnumerable<ExerciseResponse>> GenerateExercisesAsync(
        Guid lessonId,
        string lessonTitle,
        string topic,
        int count,
        ExerciseType type,
        CancellationToken cancellationToken = default);
}