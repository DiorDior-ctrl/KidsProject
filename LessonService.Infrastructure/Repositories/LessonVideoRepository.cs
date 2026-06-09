using Microsoft.EntityFrameworkCore;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Domain.Models;
using LessonService.Infrastructure.Data;

namespace LessonService.Infrastructure.Repositories
{
    public class LessonVideoRepository : ILessonVideoRepository
    {
        protected readonly LessonServiceDbContext _context;

        public LessonVideoRepository(LessonServiceDbContext context) 
        {
         _context = context;   
        }

        public async Task<LessonVideo?> GetByLessonIdAsync(Guid lessonId, CancellationToken cancellationToken = default)
        {
            return await _context.LessonVideos.FirstOrDefaultAsync(lv => lv.LessonId == lessonId, cancellationToken);
        }
        public async Task AddAsync(LessonVideo video, CancellationToken cancellationToken = default)
        {
            await _context.LessonVideos.AddAsync(video, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(LessonVideo video, CancellationToken cancellationToken = default)
        {
            _context.LessonVideos.Update(video);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(LessonVideo video, CancellationToken cancellationToken = default)
        {
            _context.LessonVideos.Remove(video);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
