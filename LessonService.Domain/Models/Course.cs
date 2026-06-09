using System;
using System.Collections.Generic;
using System.Text;
using LessonService.Domain.Exceptions;
namespace LessonService.Domain.Models
{
    public class Course
    {
        public Guid Id { get; private set; }
        public string Tittle { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int TargetAgeMin { get; private set; }
        public int TargetAgeMax { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public ICollection<Module> Modules { get; private set; } = new List<Module>();

        private Course() { }

        public static Course Create(string tittle ,string description ,int minage ,int maxage)
        {
            if (minage < 4 && maxage > 12)
                throw new BusinessException("Jashte rangut te moshes(minimalisht 4 vjec dhe maksimalisht 12 vjec.)");

            return new Course
            {
                Id = Guid.NewGuid(),
                Tittle = tittle,
                Description = description,
                TargetAgeMin = minage,
                TargetAgeMax = maxage,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
        }
        public void Update(string tittle , string description)
        {
            Tittle = tittle.Trim();
            description = description.Trim();

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
}
