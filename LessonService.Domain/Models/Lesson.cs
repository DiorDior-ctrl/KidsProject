using System;
using System.Collections.Generic;
using System.Text;
using LessonService.Domain.Models;
using LessonService.Domain.Exceptions;
namespace LessonService.Domain.Models
{
    public class Lesson
    {
        public Guid Id { get; private set; }
        public Guid ModuleId { get; private set; }
        public string Title { get; private set; }
        public int OrderIndex { get; private set; }
        public int XpReward { get; private set; }
        public bool HasVideo { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt   { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public LessonVideo? Video { get; private set; }


        public Module Module { get; private set; } = null!;
        public ICollection<Exercise> Exercises { get; private set; } = new List<Exercise>();

        private Lesson()
        {
            
        }

        public static Lesson Create(Guid moduleId ,string title , int orderindex ,int xps )
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new BusinessException("Titulli eshte bosh!");

            if (xps <= 0)
                throw new BusinessException("Piket e fituara sduhet te jene 0.");

            return new Lesson
            {
                Id = Guid.NewGuid(),
                ModuleId = moduleId,
                Title = title.Trim(),
                OrderIndex = orderindex,
                XpReward = xps,
                HasVideo = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };
        }
        public void AttachVideo(LessonVideo video)
        {
            if (video.LessonId != Id)
                throw new BusinessException("Video nuk i përket këtij leksioni.");

            Video = video;
            HasVideo = true;
        }
        public void SetVideo(bool hasVideo) => HasVideo = hasVideo;

        public void Update(string title ,int orderindex , int xps)
        {
            Title = title;
            OrderIndex = orderindex;
            XpReward = xps;
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
