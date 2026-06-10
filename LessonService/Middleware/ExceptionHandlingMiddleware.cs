using System.Net;
using System.Text.Json;
using LessonService.Domain.Exceptions;

namespace LessonService.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException ex => (HttpStatusCode.NotFound, ex.Message),
            BusinessException ex => (HttpStatusCode.BadRequest, ex.Message),
            ForbiddenException => (HttpStatusCode.Forbidden, "Nuk ke leje për këtë veprim."),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Nuk je i autorizuar."),
            _ => (HttpStatusCode.InternalServerError, "Ndodhi një gabim i brendshëm.")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Gabim i papritur: {Message}", exception.Message);

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)statusCode,
            message,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}