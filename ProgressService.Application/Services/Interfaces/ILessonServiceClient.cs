
namespace ProgressService.Application.Services.Interfaces;

// HTTP Client per te komunikuar me LessonService
public interface ILessonServiceClient
{
    Task<int> GetLessonVideoDurationAsync(Guid lessonId, CancellationToken cancellationToken = default);
    Task<int> GetLessonExerciseCountAsync(Guid lessonId, CancellationToken cancellationToken = default);
    Task<string> GetExerciseCorrectAnswerAsync(Guid exerciseId, CancellationToken cancellationToken = default);
    Task<int> GetExerciseXpRewardAsync(Guid exerciseId, CancellationToken cancellationToken = default);
}