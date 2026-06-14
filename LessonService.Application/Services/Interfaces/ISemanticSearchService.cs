using LessonService.Application.DTOs.Responses;

namespace LessonService.Application.Services.Interfaces;

public interface ISemanticSearchService
{
    Task<IEnumerable<LessonResponse>> SearchAsync(
        string query,
        int topK = 5,
        CancellationToken cancellationToken = default);

    Task IndexLessonAsync(
        Guid lessonId,
        string title,
        CancellationToken cancellationToken = default);
}