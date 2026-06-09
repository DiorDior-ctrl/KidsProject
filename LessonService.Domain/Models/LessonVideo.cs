using LessonService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Domain.Models
{
    public class LessonVideo
    {
        public Guid Id { get; private set; }
        public Guid LessonId { get; private set; }
        public string StorageKey { get; private set; } = string.Empty;
        public string StreamingUrl { get; private set; } = string.Empty ;
        public int DurationSeconds { get; private set; }
        public long FileSizeBytes { get; private set; }
        
        public VideoStatus Status { get; private set; }
        public DateTime UploadedAt {  get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt {  get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public Lesson Lesson { get; private set; } = null!;

        private LessonVideo()
        {
             
        }

        public static LessonVideo Create(Guid lessonid , string storagekey , long filesize)
        {
            return new LessonVideo
            {
                Id = Guid.NewGuid(),
                LessonId = lessonid,
                StorageKey = storagekey,
                StreamingUrl = string.Empty,
                FileSizeBytes = filesize,
                Status = VideoStatus.Processing,
                UploadedAt = DateTime.UtcNow
            };
        }
        public void MarkAsReady(string streamingUrl, int durationSeconds)
        {
            StreamingUrl = streamingUrl;
            DurationSeconds = durationSeconds;
            Status = VideoStatus.Ready;
            ProcessedAt = DateTime.UtcNow;
        }
        public void MarkAsFailed()
        {
            Status = VideoStatus.Failed;
            ProcessedAt = DateTime.UtcNow;
        }
        public void SoftDelete() => DeletedAt = DateTime.UtcNow;
    }
}
