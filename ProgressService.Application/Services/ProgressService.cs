using Microsoft.Extensions.Logging;
using ProgressService.Application.DTOs.Requests;
using ProgressService.Application.DTOs.Responses;
using ProgressService.Application.Repositories.Interfaces;
using ProgressService.Application.Services.Interfaces;
using ProgressService.Domain.Exceptions;
using ProgressService.Domain.Models;

namespace ProgressService.Application.Services;

public class ProgressService : IProgressService
{
    private readonly ILessonSessionRepository _sessionRepository;
    private readonly IExerciseAttemptRepository _attemptRepository;
    private readonly IUserProgressRepository _progressRepository;
    private readonly ILessonServiceClient _lessonClient;
    private readonly ILogger<ProgressService> _logger;

    public ProgressService(
        ILessonSessionRepository sessionRepository,
        IExerciseAttemptRepository attemptRepository,
        IUserProgressRepository progressRepository,
        ILessonServiceClient lessonClient,
        ILogger<ProgressService> logger)
    {
        _sessionRepository = sessionRepository;
        _attemptRepository = attemptRepository;
        _progressRepository = progressRepository;
        _lessonClient = lessonClient;
        _logger = logger;
    }

    public async Task<LessonSessionResponse> StartLessonAsync(
        Guid userId,
        StartLessonRequest request,
        CancellationToken cancellationToken = default)
    {
        // Kontrollo nëse ka sesion aktiv për këtë leksion
        var existingSession = await _sessionRepository.GetActiveSessionAsync(
            userId, request.LessonId, cancellationToken);

        if (existingSession != null)
        {
            return MapToResponse(existingSession);
        }

        // Krijo sesion të ri
        var session = LessonSession.Start(userId, request.LessonId);
        await _sessionRepository.AddAsync(session, cancellationToken);

        _logger.LogInformation("Sesion i ri u krijua: {SessionId} për userin {UserId}",
            session.Id, userId);

        return MapToResponse(session);
    }

    public async Task<VideoProgressResponse> UpdateVideoProgressAsync(
        Guid sessionId,
        Guid userId,
        UpdateVideoProgressRequest request,
        CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken)
            ?? throw new NotFoundException("Session", sessionId);

        if (session.UserId != userId)
            throw new ForbiddenException();

        session.UpdateVideoProgress(request.CurrentSeconds, request.TotalSeconds);
        await _sessionRepository.UpdateAsync(session, cancellationToken);

        return new VideoProgressResponse(
            session.VideoCompleted,
            session.VideoCompleted,
            session.VideoProgressSeconds);
    }

    public async Task<ExerciseResultResponse> SubmitAnswerAsync(
        Guid sessionId,
        Guid userId,
        SubmitAnswerRequest request,
        CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByIdWithAttemptsAsync(sessionId, cancellationToken)
            ?? throw new NotFoundException("Session", sessionId);

        if (session.UserId != userId)
            throw new ForbiddenException();

        if (!session.VideoCompleted)
            throw new BusinessException("Duhet të shikosh videon fillimisht!");

        // Merr përgjigjen e saktë nga LessonService
        var correctAnswer = await _lessonClient.GetExerciseCorrectAnswerAsync(
            request.ExerciseId, cancellationToken);

        var xpReward = await _lessonClient.GetExerciseXpRewardAsync(
            request.ExerciseId, cancellationToken);

        bool isCorrect = string.Equals(
            request.Answer.Trim(),
            correctAnswer.Trim(),
            StringComparison.OrdinalIgnoreCase);

        int xpEarned = isCorrect ? xpReward : 0;

        // Regjistro tentativën
        var attempt = ExerciseAttempt.Create(
            sessionId,
            request.ExerciseId,
            request.Answer,
            isCorrect,
            xpEarned,
            request.TimeTakenMs);

        await _attemptRepository.AddAsync(attempt, cancellationToken);
        session.RecordExerciseXp(xpEarned);

        // Kontrollo nëse është ushtrimi i fundit
        var totalExercises = await _lessonClient.GetLessonExerciseCountAsync(
            session.LessonId, cancellationToken);

        var completedAttempts = await _attemptRepository.CountBySessionIdAsync(
            sessionId, cancellationToken);

        bool isLastExercise = completedAttempts >= totalExercises;

        LessonCompletionResponse? completion = null;

        if (isLastExercise)
        {
            session.Complete();

            // Përditëso progresin e userit
            var userProgress = await _progressRepository.GetByUserIdAsync(userId, cancellationToken);

            if (userProgress == null)
            {
                userProgress = UserProgress.Create(userId);
                userProgress.AddXp(session.TotalXpEarned);
                userProgress.UpdateStreak();
                await _progressRepository.AddAsync(userProgress, cancellationToken);
            }
            else
            {
                userProgress.AddXp(session.TotalXpEarned);
                userProgress.UpdateStreak();
                await _progressRepository.UpdateAsync(userProgress, cancellationToken);
            }

            completion = new LessonCompletionResponse(
                session.TotalXpEarned,
                userProgress.CurrentStreak,
                userProgress.TotalXp);

            _logger.LogInformation("Leksioni u kompletua: {SessionId} nga useri {UserId}",
                sessionId, userId);
        }

        await _sessionRepository.UpdateAsync(session, cancellationToken);

        return new ExerciseResultResponse(
            isCorrect,
            isCorrect ? "Shumë mirë! 🌟" : "Provo përsëri!",
            xpEarned,
            isLastExercise,
            completion);
    }

    public async Task<IEnumerable<LessonSessionResponse>> GetUserSessionsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetByUserIdAsync(userId, cancellationToken);
        return sessions.Select(MapToResponse);
    }

    public async Task<UserProgressResponse> GetUserProgressAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var progress = await _progressRepository.GetByUserIdAsync(userId, cancellationToken);

        if (progress == null)
        {
            return new UserProgressResponse(userId, 0, 0, 0, null);
        }

        return new UserProgressResponse(
            progress.UserId,
            progress.TotalXp,
            progress.CurrentStreak,
            progress.LongestStreak,
            progress.LastActivityDate);
    }

    private static LessonSessionResponse MapToResponse(LessonSession session)
    {
        return new LessonSessionResponse(
            session.Id,
            session.LessonId,
            session.Status.ToString(),
            session.VideoCompleted,
            session.VideoProgressSeconds,
            session.TotalXpEarned,
            session.StartedAt);
    }
}