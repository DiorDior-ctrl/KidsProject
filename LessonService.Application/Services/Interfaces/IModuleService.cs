
using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;

namespace LessonService.Application.Services.Interfaces;

public interface IModuleService
{
    Task<IEnumerable<ModuleResponse>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default);
    Task<ModuleResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ModuleResponse> CreateAsync(CreateModuleRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}