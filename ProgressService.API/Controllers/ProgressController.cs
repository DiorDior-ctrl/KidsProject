using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ProgressService.Application.DTOs.Requests;
using ProgressService.Application.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;
namespace ProgressService.API.Controllers
{
    [ApiController]
    [Route("api/v1/progress")]
    [Authorize]
    [EnableRateLimiting("GeneralPolicy")]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService progress;

        public ProgressController(IProgressService service)
        {
            progress = service;
        }
        private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException());


        [HttpPost("sessions/start")]
        [Authorize(Policy = "ChildOnly")]
        public async Task<IActionResult> StartLesson(
        [FromBody] StartLessonRequest request,
        CancellationToken cancellationToken)
        {
            var reult = await progress.StartLessonAsync(GetUserId(), request, cancellationToken);
            return Ok(reult);
        }

        //Frontendi dergon progresin e videos cdo 15 sekonda
        [HttpPost("sessions/{sessionId}/video-progress")]
        [Authorize(Policy = "ChildOnly")]
        public async Task<IActionResult> UpdateVideoProgress(
        Guid sessionId,
        [FromBody] UpdateVideoProgressRequest request,
        CancellationToken cancellationToken)
        {
            var result = await progress.UpdateVideoProgressAsync(sessionId, GetUserId(), request, cancellationToken);
            return Ok(result);
        }

        //Femija dergon pergjigjet per ushtrimet
        [HttpPost("sessions/{sessionId}/submit-answer")]
        [Authorize(Policy = "ChildOnly")]
        public async Task<IActionResult> SubmitAnswer(
        Guid sessionId,
        [FromBody] SubmitAnswerRequest request,
        CancellationToken cancellationToken)
        {
            var result = await progress.SubmitAnswerAsync(sessionId , GetUserId(), request, cancellationToken);
            return Ok(result);
        }

        // Merr te gjitha sesionet e userit
        [HttpGet("sessions")]
        public async Task<IActionResult> GetMySessions(CancellationToken cancellationToken)
        {
            var result = await progress.GetUserSessionsAsync(GetUserId(), cancellationToken);
            return Ok(result);
        }

        //Merr progresin total te userit
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProgress(CancellationToken cancellationToken)
        {
            var result = progress.GetUserProgressAsync(GetUserId(), cancellationToken);
            return Ok(result);
        }

        //Prindi sheh progresin e femijes
        [HttpGet("users/{userId}")]
        [Authorize(Policy = "ParentOrAdmin")]
        public async Task<IActionResult> GetUserProgress(
        Guid userId,
        CancellationToken cancellationToken)
        {
            var res = await progress.GetUserProgressAsync(userId, cancellationToken);
            return Ok(res);
        }


    }
}
