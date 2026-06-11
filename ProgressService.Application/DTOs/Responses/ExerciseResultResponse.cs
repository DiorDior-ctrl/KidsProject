
namespace ProgressService.Application.DTOs.Responses;

public record ExerciseResultResponse(
    bool IsCorrect,
    string FeedbackMessage,
    int XpEarned,
    bool IsLastExercise,
    LessonCompletionResponse? Completion
);