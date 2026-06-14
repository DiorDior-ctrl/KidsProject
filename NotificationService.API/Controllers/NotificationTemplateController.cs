
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTOs.Requests;
using NotificationService.Application.Services.Interfaces;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/v1/notification-templates")]
[Authorize(Policy = "AdminOnly")]
public class NotificationTemplateController : ControllerBase
{
    private readonly INotificationTemplateService _templateService;

    public NotificationTemplateController(INotificationTemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _templateService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _templateService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _templateService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}