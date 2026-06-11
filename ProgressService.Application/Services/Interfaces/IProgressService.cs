
using ProgressService.Application.DTOs.Requests;
using ProgressService.Application.DTOs.Responses;

namespace ProgressService.Application.Services.Interfaces;

public interface IProgressService
{
    Task<LessonSessionResponse> StartLessonAsync(Guid userId, StartLessonRequest request, CancellationToken cancellationToken = default);
    Task<VideoProgressResponse> UpdateVideoProgressAsync(Guid sessionId, Guid userId, UpdateVideoProgressRequest request, CancellationToken cancellationToken = default);
    Task<ExerciseResultResponse> SubmitAnswerAsync(Guid sessionId, Guid userId, SubmitAnswerRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<LessonSessionResponse>> GetUserSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserProgressResponse> GetUserProgressAsync(Guid userId, CancellationToken cancellationToken = default);
}