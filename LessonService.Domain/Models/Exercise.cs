using LessonService.Domain.Models;
using LessonService.Domain.Enums;
using LessonService.Domain.Exceptions;
namespace LessonService.Domain.Models;

public class Exercise
{
    public Guid Id { get; private set; }
    public Guid LessonId { get; private set; }
    public ExerciseType Type { get; private set; }
    public int OrderIndex { get; private set; }
    public string ContentJson { get; private set; } = string.Empty;
    public string CorrectAnswer { get; private set; } = string.Empty;
    public int XpReward { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public Lesson Lesson { get; private set; } = null!;

    private Exercise() { }

    public static Exercise Create(
        Guid lessonId,
        ExerciseType type,
        int orderIndex,
        string contentJson,
        string correctAnswer,
        int xpReward)
    {
        if (string.IsNullOrWhiteSpace(contentJson))
            throw new BusinessException("Content nuk mund të jetë bosh.");

        if (string.IsNullOrWhiteSpace(correctAnswer))
            throw new BusinessException("Përgjigja e saktë nuk mund të jetë bosh.");

        if (xpReward <= 0)
            throw new BusinessException("XP reward duhet të jetë më i madh se 0.");

        return new Exercise
        {
            Id = Guid.NewGuid(),
            LessonId = lessonId,
            Type = type,
            OrderIndex = orderIndex,
            ContentJson = contentJson,
            CorrectAnswer = correctAnswer,
            XpReward = xpReward,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SoftDelete() => DeletedAt = DateTime.UtcNow;
}