using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Repositories.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Data;
namespace NotificationService.Infrastructure.Repositories
{
    public class NotificationTemplateRepository : INotificationTemplateRepository
    {
        private readonly NotificationServiceDbContext _context;


        public NotificationTemplateRepository(NotificationServiceDbContext context)
        {
            _context = context;
        }
        public async Task<NotificationTemplate?> GetByTypeAsync(NotificationType type, CancellationToken cancellationToken = default)
        {
            return await _context.NotificationTemplates
                .FirstOrDefaultAsync(nt => nt.Type == type, cancellationToken);
        }
        public async Task<IEnumerable<NotificationTemplate>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.NotificationTemplates
                .OrderBy(nt => nt.Type)
                .ToListAsync(cancellationToken);
        }
        public async Task AddAsync(NotificationTemplate template, CancellationToken cancellationToken = default)
        {
            await _context.NotificationTemplates.AddAsync(template, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(NotificationTemplate template, CancellationToken cancellationToken = default)
        {
            template.SoftDelete();
            _context.NotificationTemplates.Update(template);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }


}
