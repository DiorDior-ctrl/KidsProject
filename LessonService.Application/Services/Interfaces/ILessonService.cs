
using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;

namespace LessonService.Application.Services.Interfaces;

public interface ILessonService
{
    Task<IEnumerable<LessonResponse>> GetByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken = default);
    Task<LessonDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LessonResponse> CreateAsync(CreateLessonRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}