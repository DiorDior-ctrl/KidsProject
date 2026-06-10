using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Application.Services.Interfaces;
using LessonService.Domain.Exceptions;
using LessonService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace LessonService.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly ILogger<LessonService> _logger;

        public LessonService(
            ILessonRepository lessonRepository,
            IModuleRepository moduleRepository,
            ILogger<LessonService> logger)
        {
            _lessonRepository = lessonRepository;
            _moduleRepository = moduleRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<LessonResponse>> GetByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken = default)
        {
            var lessons = await _lessonRepository.GetByModuleIdAsync(moduleId, cancellationToken);
            return lessons.Select(l => new LessonResponse(
                l.Id, l.ModuleId, l.Title,
                l.OrderIndex, l.XpReward, l.HasVideo, l.CreatedAt));
        }
        public async Task<LessonDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var lesson = await _lessonRepository.GetByIdWithExercisesAsync(id, cancellationToken)
            ?? throw new NotFoundException("Lesson", id);

            var videoResponse = lesson.Video != null
                ? new VideoResponse(
                    lesson.Video.Id,
                    lesson.Video.StreamingUrl,
                    lesson.Video.DurationSeconds,
                    lesson.Video.Status.ToString())
                : null;

            var exercises = lesson.Exercises.Select(e => new ExerciseResponse(
                e.Id, e.LessonId, e.Type,
                e.OrderIndex, e.ContentJson, e.XpReward));

            return new LessonDetailResponse(
                lesson.Id, lesson.ModuleId, lesson.Title,
                lesson.OrderIndex, lesson.XpReward, lesson.HasVideo,
                videoResponse, exercises, lesson.CreatedAt);
        }

        public async Task<LessonResponse> CreateAsync(CreateLessonRequest request, CancellationToken cancellationToken = default)
        {
            
            var module = await _moduleRepository.GetByIdAsync(request.ModuleId, cancellationToken)
                ?? throw new NotFoundException("Module", request.ModuleId);

            var lesson = Lesson.Create(
                request.ModuleId,
                request.Title,
                request.OrderIndex,
                request.XpReward);

            await _lessonRepository.AddAsync(lesson, cancellationToken);

            _logger.LogInformation("Lesson u krijua: {Title} për modulin {ModuleId}",
                lesson.Title, request.ModuleId);

            return new LessonResponse(
                lesson.Id, lesson.ModuleId, lesson.Title,
                lesson.OrderIndex, lesson.XpReward, lesson.HasVideo, lesson.CreatedAt);
        }
        
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Lesson" , id);

            lesson.SoftDelete();
            await _lessonRepository.UpdateAsync(lesson, cancellationToken);
            _logger.LogInformation($"Lesson u fshi:" , id);
        }
    }
}
