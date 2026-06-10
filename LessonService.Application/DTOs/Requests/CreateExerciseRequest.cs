using LessonService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Application.DTOs.Requests
{
    public record CreateExerciseRequest(
    Guid LessonId,
    ExerciseType Type,
    int OrderIndex,
    string ContentJson,
    string CorrectAnswer,
    int XpReward
    );
}
