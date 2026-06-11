
namespace ProgressService.Application.DTOs.Requests;

public record SubmitAnswerRequest(
    Guid ExerciseId,
    string Answer,
    int TimeTakenMs
);