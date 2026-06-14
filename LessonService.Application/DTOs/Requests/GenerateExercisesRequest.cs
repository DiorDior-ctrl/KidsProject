using LessonService.Domain.Enums;

namespace LessonService.Application.DTOs.Requests;

public record GenerateExercisesRequest(
    Guid LessonId,
    string LessonTitle,
    string Topic,
    int Count,
    ExerciseType Type
);