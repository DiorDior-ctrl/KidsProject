
using LessonService.Application.DTOs.Requests;
using LessonService.Application.DTOs.Responses;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Application.Services.Interfaces;
using LessonService.Domain.Exceptions;
using LessonService.Domain.Models;
using Microsoft.Extensions.Logging;
namespace LessonService.Application.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ILogger<ModuleService> _logger;
        private readonly ICourseRepository _courseRepository;

        public ModuleService(IModuleRepository moduleRepository , ILogger<ModuleService> logger 
            ,ICourseRepository courseRepository )
        {
            _moduleRepository = moduleRepository;
            _logger = logger;
            _courseRepository = courseRepository;
        }


        public async Task<IEnumerable<ModuleResponse>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default)
        {
            var module = await _moduleRepository.GetByCourseIdAsync(courseId, cancellationToken);
            return module.Select(m => new ModuleResponse(
            m.Id, m.CourseId, m.Title, m.OrderIndex, m.CreatedAt));
                
        }
        public async Task<ModuleResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var module = await _moduleRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Module", id);

            return new ModuleResponse(module.Id, module.CourseId, module.Title,
            module.OrderIndex, module.CreatedAt);
        }
        public async Task<ModuleResponse> CreateAsync(CreateModuleRequest request, CancellationToken cancellationToken = default)
        {
            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
                    ?? throw new NotFoundException("Course", request.CourseId);

            var module = Module.Create(request.CourseId, request.Title, request.OrderIndex);
            await _moduleRepository.AddAsync(module, cancellationToken);

            _logger.LogInformation("Module u krijua: { Title}për kursin { CourseId}", module.Title, request.CourseId);
            
            return new ModuleResponse(
            module.Id, module.CourseId, module.Title,
            module.OrderIndex, module.CreatedAt);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var module = await _moduleRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Module", id);

            module.SoftDelete();
            await _moduleRepository.UpdateAsync(module, cancellationToken);

            _logger.LogInformation("Module u fshi: {Id}", id);
        }
    }
}
