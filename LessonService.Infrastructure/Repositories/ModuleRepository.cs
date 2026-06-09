using Microsoft.EntityFrameworkCore;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Domain.Models;
using LessonService.Infrastructure.Data;

namespace LessonService.Infrastructure.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        protected readonly LessonServiceDbContext _context;

        public ModuleRepository(LessonServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Module?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Modules.FirstOrDefaultAsync(m => m.Id == id , cancellationToken);
        }
        public async Task<IEnumerable<Module>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default)
        {
            return await _context.Modules.Where(m => m.CourseId == courseId).OrderBy(m => m.OrderIndex).ToListAsync();
        }
        public async Task AddAsync(Module module, CancellationToken cancellationToken = default)
        {
            await _context.Modules.AddAsync(module, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateAsync(Module module, CancellationToken cancellationToken = default)
        {
            _context.Modules.Update(module);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Module module, CancellationToken cancellationToken = default)
        {
            _context.Modules.Remove(module);
            await _context.SaveChangesAsync(cancellationToken) ;
        }
    }
}
