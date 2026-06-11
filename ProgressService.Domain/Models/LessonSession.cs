
using ProgressService.Domain.Enums;
using ProgressService.Domain.Exceptions;

namespace ProgressService.Domain.Models;

public class LessonSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid LessonId { get; private set; }
    public LessonSessionStatus Status { get; private set; }
    public int VideoProgressSeconds { get; private set; }
    public bool VideoCompleted { get; private set; }
    public int TotalXpEarned { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public ICollection<ExerciseAttempt> Attempts { get; private set; } = new List<ExerciseAttempt>();

    private LessonSession() { }

    public static LessonSession Start(Guid userId, Guid lessonId)
    {
        return new LessonSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LessonId = lessonId,
            Status = LessonSessionStatus.WatchingVideo,
            VideoProgressSeconds = 0,
            VideoCompleted = false,
            TotalXpEarned = 0,
            StartedAt = DateTime.UtcNow
        };
    }

    public void UpdateVideoProgress(int currentSeconds, int totalSeconds)
    {
        if (currentSeconds < 0)
            throw new BusinessException("Sekonda nuk mund të jetë negative.");

        VideoProgressSeconds = currentSeconds;

        double percentage = (double)currentSeconds / totalSeconds * 100;
        if (percentage >= 85 && !VideoCompleted)
        {
            VideoCompleted = true;
            Status = LessonSessionStatus.DoingExercises;
        }
    }

    public void RecordExerciseXp(int xp)
    {
        if (xp < 0)
            throw new BusinessException("XP nuk mund të jetë negative.");

        TotalXpEarned += xp;
    }

    public void Complete()
    {
        Status = LessonSessionStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public bool IsVideoRequired() => Status == LessonSessionStatus.WatchingVideo;
}