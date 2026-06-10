using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LessonService.Application.DTOs.Requests;
using LessonService.Application.Services.Interfaces;
namespace LessonService.API.Controllers
{
    [ApiController]
    [Route("api/v1/modules")]
    [Authorize]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetByCourseId(Guid courseId, CancellationToken cancellationToken)
        {
            var result = await _moduleService.GetByCourseIdAsync(courseId, cancellationToken);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _moduleService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(
        [FromBody] CreateModuleRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _moduleService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById) , new {id = result.Id} , result);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _moduleService.DeleteAsync(id, cancellationToken);
            return NoContent(); 
        }
    }
}
