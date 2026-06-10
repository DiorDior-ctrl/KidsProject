
using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;

namespace LessonService.Application.Services.Interfaces;

public interface IExerciseService
{
    Task<IEnumerable<ExerciseResponse>> GetByLessonIdAsync(Guid lessonId, CancellationToken cancellationToken = default);
    Task<ExerciseResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ExerciseResponse> CreateAsync(CreateExerciseRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}