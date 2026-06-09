
using LessonService.Domain.Models;

namespace LessonService.Application.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Course course, CancellationToken cancellationToken = default);
    Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
    Task DeleteAsync(Course course, CancellationToken cancellationToken = default);
}