
namespace ProgressService.Domain.Models;

public class UserProgress
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public int TotalXp { get; private set; }
    public int CurrentStreak { get; private set; }
    public int LongestStreak { get; private set; }
    public DateTime? LastActivityDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private UserProgress() { }

    public static UserProgress Create(Guid userId)
    {
        return new UserProgress
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TotalXp = 0,
            CurrentStreak = 0,
            LongestStreak = 0,
            LastActivityDate = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void AddXp(int xp)
    {
        if (xp <= 0) return;
        TotalXp += xp;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStreak()
    {
        var today = DateTime.UtcNow.Date;

        if (LastActivityDate == null || LastActivityDate.Value.Date < today.AddDays(-1))
        {
            // Streak prishet ose fillon nga e para
            CurrentStreak = 1;
        }
        else if (LastActivityDate.Value.Date == today.AddDays(-1))
        {
            // Dite radhazi
            CurrentStreak++;
        }
        // Nese LastActivityDate eshte sot, mos ndrysho streak

        if (CurrentStreak > LongestStreak)
            LongestStreak = CurrentStreak;

        LastActivityDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}