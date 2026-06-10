using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Application.Services.Interfaces;
using LessonService.Domain.Exceptions;
using LessonService.Domain.Models;
using Microsoft.Extensions.Logging;
namespace LessonService.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository courseRepository , ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<CourseResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var courses = await _courseRepository.GetAllAsync(cancellationToken);
            return courses.Select(c => new CourseResponse(
                c.Id, c.Title, c.Description, c.TargetAgeMin, c.TargetAgeMax, c.IsActive, c.CreatedAt));
        }
        public async Task<CourseResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Course", id);

            return new CourseResponse(
            course.Id, course.Title, course.Description,
            course.TargetAgeMin, course.TargetAgeMax,
            course.IsActive, course.CreatedAt);
        }
        public async Task<CourseResponse> CreateAsync(CreateCourseRequest request, CancellationToken cancellationToken = default)
        {
            var course = Course.Create(
                request.Title, request.Description, request.TargetAgeMin, request.TargetAgeMax);

            await _courseRepository.AddAsync(course , cancellationToken);
            _logger.LogInformation("Kursi u krijua: {title}", course.Title);

            return new CourseResponse(
            course.Id, course.Title, course.Description,
            course.TargetAgeMin, course.TargetAgeMax,
            course.IsActive, course.CreatedAt);
        }
        public async Task<CourseResponse> UpdateAsync(Guid id, UpdateCourseRequest request, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Course" , id);

            course.Update(request.Title, request.Description);
            await _courseRepository.UpdateAsync(course , cancellationToken);
            _logger.LogInformation("Kursi u perditesua : {Id}", id);

            return new CourseResponse(
            course.Id, course.Title, course.Description,
            course.TargetAgeMin, course.TargetAgeMax,
            course.IsActive, course.CreatedAt);

        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Kursi nuk u gjet!");

            course.SoftDelete();
            await _courseRepository.DeleteAsync(course , cancellationToken);

            _logger.LogInformation("Kursi u fshi {Id}", id);
        }
    }
}
