using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Application.Services.Interfaces;
using LessonService.Domain.Exceptions;
using LessonService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace LessonService.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CourseService> _logger;

    public CourseService(
        ICourseRepository courseRepository,
        ICacheService cacheService,
        ILogger<CourseService> logger)
    {
        _courseRepository = courseRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<IEnumerable<CourseResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Cache-Aside Pattern
        var cached = await _cacheService.GetAsync<IEnumerable<CourseResponse>>(
            CacheKeys.AllCourses, cancellationToken);

        if (cached != null)
            return cached;

        var courses = await _courseRepository.GetAllAsync(cancellationToken);
        var response = courses.Select(c => new CourseResponse(
            c.Id, c.Title, c.Description,
            c.TargetAgeMin, c.TargetAgeMax,
            c.IsActive, c.CreatedAt)).ToList();

        await _cacheService.SetAsync(CacheKeys.AllCourses, response,
            TimeSpan.FromHours(1), cancellationToken);

        return response;
    }

    public async Task<CourseResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cached = await _cacheService.GetAsync<CourseResponse>(
            CacheKeys.Course(id), cancellationToken);

        if (cached != null)
            return cached;

        var course = await _courseRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Course", id);

        var response = new CourseResponse(
            course.Id, course.Title, course.Description,
            course.TargetAgeMin, course.TargetAgeMax,
            course.IsActive, course.CreatedAt);

        await _cacheService.SetAsync(CacheKeys.Course(id), response,
            TimeSpan.FromHours(1), cancellationToken);

        return response;
    }

    public async Task<CourseResponse> CreateAsync(CreateCourseRequest request, CancellationToken cancellationToken = default)
    {
        var course = Course.Create(
            request.Title,
            request.Description,
            request.TargetAgeMin,
            request.TargetAgeMax);

        await _courseRepository.AddAsync(course, cancellationToken);

        // Invalidate cache
        await _cacheService.RemoveAsync(CacheKeys.AllCourses, cancellationToken);

        _logger.LogInformation("Kursi u krijua: {Title}", course.Title);

        return new CourseResponse(
            course.Id, course.Title, course.Description,
            course.TargetAgeMin, course.TargetAgeMax,
            course.IsActive, course.CreatedAt);
    }

    public async Task<CourseResponse> UpdateAsync(Guid id, UpdateCourseRequest request, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Course", id);

        course.Update(request.Title, request.Description);
        await _courseRepository.UpdateAsync(course, cancellationToken);

        // Invalidate cache
        await _cacheService.RemoveAsync(CacheKeys.Course(id), cancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.AllCourses, cancellationToken);

        _logger.LogInformation("Kursi u përditësua: {Id}", id);

        return new CourseResponse(
            course.Id, course.Title, course.Description,
            course.TargetAgeMin, course.TargetAgeMax,
            course.IsActive, course.CreatedAt);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Course", id);

        course.SoftDelete();
        await _courseRepository.UpdateAsync(course, cancellationToken);

        // Invalidate cache
        await _cacheService.RemoveAsync(CacheKeys.Course(id), cancellationToken);
        await _cacheService.RemoveAsync(CacheKeys.AllCourses, cancellationToken);

        _logger.LogInformation("Kursi u fshi: {Id}", id);
    }
}