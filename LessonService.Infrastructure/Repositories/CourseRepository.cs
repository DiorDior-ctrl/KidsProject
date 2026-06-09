using LessonService.Application.Repositories.Interfaces;
using LessonService.Domain.Models;
using LessonService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LessonService.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        protected readonly LessonServiceDbContext _context;
        public CourseRepository(LessonServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Course> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id , cancellationToken);
        }
        public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Courses.OrderBy(c => c.Tittle).ToListAsync();
        }
        public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
        {
            await _context.Courses.AddAsync(course, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Course course, CancellationToken cancellationToken = default)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync(cancellationToken) ;
        }
    }
}
