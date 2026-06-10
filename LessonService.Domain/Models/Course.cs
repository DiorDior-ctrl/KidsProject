using LessonService.Domain.Exceptions;

namespace LessonService.Domain.Models;

public class Course
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int TargetAgeMin { get; private set; }
    public int TargetAgeMax { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public ICollection<Module> Modules { get; private set; } = new List<Module>();

    private Course() { }

    public static Course Create(string title, string description, int targetAgeMin, int targetAgeMax)
    {
        if (targetAgeMin < 4 || targetAgeMax > 12)
            throw new BusinessException("Mosha duhet të jetë ndërmjet 4 dhe 12 vjeç.");

        if (targetAgeMin >= targetAgeMax)
            throw new BusinessException("Mosha minimale duhet të jetë më e vogël se maksimalja.");

        return new Course
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Description = description.Trim(),
            TargetAgeMin = targetAgeMin,
            TargetAgeMax = targetAgeMax,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string title, string description)
    {
        Title = title.Trim();
        Description = description.Trim();
    }

    public void SoftDelete()
    {
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        DeletedAt = null;
    }
}