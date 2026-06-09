using System;
using System.Collections.Generic;
using System.Text;
using LessonService.Domain.Exceptions;
namespace LessonService.Domain.Models
{
    public class Module
    {
        public Guid Id { get; private set; }
        public Guid CourseId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public int OrderIndex { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public Course Course { get; private set; } = null!;
        public ICollection<Lesson> Lessons { get; private set; } = new List<Lesson>();

        private Module()
        {
            
        }

        public static Module Create(Guid courseid ,string title ,int orderindex)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new BusinessException("Titulli nuk u gjet!");

            return new Module
            {
                Id = Guid.NewGuid(),
                CourseId = courseid,
                Title = title.Trim(),
                OrderIndex = orderindex,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }
        public void Update(string tittle ,int orderindex)
        {
            Title = tittle;
            OrderIndex = orderindex;
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
