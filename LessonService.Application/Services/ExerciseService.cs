
using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Application.Services.Interfaces;
using LessonService.Domain.Exceptions;
using LessonService.Domain.Models;
using Microsoft.Extensions.Logging;
namespace LessonService.Application.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILogger<ExerciseService> _logger;

        public ExerciseService(
        IExerciseRepository exerciseRepository,
        ILessonRepository lessonRepository,
        ILogger<ExerciseService> logger)
        {
            _exerciseRepository = exerciseRepository;
            _lessonRepository = lessonRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ExerciseResponse>> GetByLessonIdAsync(Guid lessonId, CancellationToken cancellationToken = default)
        {
            var exercise = await _exerciseRepository.GetByLessonIdAsync(lessonId, cancellationToken)
                ?? throw new NotFoundException("Exercise" , lessonId);
            return exercise.Select(e => new ExerciseResponse(
           e.Id, e.LessonId, e.Type,
           e.OrderIndex, e.ContentJson, e.XpReward,e.CorrectAnswer));

        }
        public async Task<ExerciseResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Exercise" ,id);
            return new ExerciseResponse(
            exercise.Id, exercise.LessonId, exercise.Type,
            exercise.OrderIndex, exercise.ContentJson, exercise.XpReward,exercise.CorrectAnswer);

        }
        public async Task<ExerciseResponse> CreateAsync(CreateExerciseRequest request, CancellationToken cancellationToken = default)
        {
            var lesson = await _lessonRepository.GetByIdAsync(request.LessonId, cancellationToken)
            ?? throw new NotFoundException("Lesson", request.LessonId);

            var exercise = Exercise.Create(
            request.LessonId,
            request.Type,
            request.OrderIndex,
            request.ContentJson,
            request.CorrectAnswer,
            request.XpReward
            
            );

            await _exerciseRepository.AddAsync(exercise, cancellationToken);

            _logger.LogInformation("Exercise u krijua për leksionin {LessonId}", request.LessonId);

            return new ExerciseResponse(
                exercise.Id, exercise.LessonId, exercise.Type,
                exercise.OrderIndex, exercise.ContentJson, exercise.XpReward , exercise.CorrectAnswer
                );
            

        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(id , cancellationToken)
                ?? throw new NotFoundException("Exercise" , id);

            exercise.SoftDelete();
            await _exerciseRepository.UpdateAsync(exercise, cancellationToken);

            _logger.LogInformation("Exercise u fshi: {Id}", id);
        }
    }
}
