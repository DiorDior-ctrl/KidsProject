using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LessonService.Application.DTOs.Requests;
using LessonService.Application.Services.Interfaces;
using Microsoft.AspNetCore.RateLimiting;

namespace LessonService.API.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    [Authorize]
    [EnableRateLimiting("GeneralPolicy")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _courseService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _courseService.GetByIdAsync(id, cancellationToken);  
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(
        [FromBody] CreateCourseRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _courseService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCourseRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _courseService.UpdateAsync(id, request, cancellationToken);
            return NoContent();
        }

    }
}
