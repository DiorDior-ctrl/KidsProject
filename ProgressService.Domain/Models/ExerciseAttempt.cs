
using ProgressService.Domain.Exceptions;

namespace ProgressService.Domain.Models;

public class ExerciseAttempt
{
    public Guid Id { get; private set; }
    public Guid LessonSessionId { get; private set; }
    public Guid ExerciseId { get; private set; }
    public string AnswerGiven { get; private set; } = string.Empty;
    public bool IsCorrect { get; private set; }
    public int XpEarned { get; private set; }
    public int TimeTakenMs { get; private set; }
    public DateTime AttemptedAt { get; private set; }

    public LessonSession LessonSession { get; private set; } = null!;

    private ExerciseAttempt() { }

    public static ExerciseAttempt Create(
        Guid sessionId,
        Guid exerciseId,
        string answerGiven,
        bool isCorrect,
        int xpEarned,
        int timeTakenMs)
    {
        if (string.IsNullOrWhiteSpace(answerGiven))
            throw new BusinessException("Përgjigja nuk mund të jetë bosh.");

        if (timeTakenMs < 0)
            throw new BusinessException("Koha nuk mund të jetë negative.");

        return new ExerciseAttempt
        {
            Id = Guid.NewGuid(),
            LessonSessionId = sessionId,
            ExerciseId = exerciseId,
            AnswerGiven = answerGiven,
            IsCorrect = isCorrect,
            XpEarned = xpEarned,
            TimeTakenMs = timeTakenMs,
            AttemptedAt = DateTime.UtcNow
        };
    }
}