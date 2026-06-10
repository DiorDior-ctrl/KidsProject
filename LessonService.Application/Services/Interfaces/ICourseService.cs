
using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;

namespace LessonService.Application.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CourseResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CourseResponse> CreateAsync(CreateCourseRequest request, CancellationToken cancellationToken = default);
    Task<CourseResponse> UpdateAsync(Guid id, UpdateCourseRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}