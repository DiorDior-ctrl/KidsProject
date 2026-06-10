
using LessonService.Domain.Enums;

namespace LessonService.Application.DTOs.Responses;

public record ExerciseResponse(
    Guid Id,
    Guid LessonId,
    ExerciseType Type,
    int OrderIndex,
    string ContentJson,
    int XpReward
);