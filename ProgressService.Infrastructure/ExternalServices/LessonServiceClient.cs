using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProgressService.Application.Services.Interfaces;
using ProgressService.Domain.Exceptions;

namespace ProgressService.Infrastructure.ExternalServices;

public class LessonServiceClient : ILessonServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<LessonServiceClient> _logger;

    public LessonServiceClient(
        HttpClient httpClient,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ILogger<LessonServiceClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["LessonService:BaseUrl"]
            ?? "http://localhost:5001");
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    private void AddAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Replace("Bearer ", "");

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<int> GetLessonVideoDurationAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync(
            $"/api/v1/lessons/{lessonId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return 0;

        var lesson = await response.Content.ReadFromJsonAsync<LessonDto>(
            cancellationToken: cancellationToken);

        return lesson?.Video?.DurationSeconds ?? 0;
    }

    public async Task<int> GetLessonExerciseCountAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync(
            $"/api/v1/exercises/lesson/{lessonId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return 0;

        var exercises = await response.Content.ReadFromJsonAsync<List<ExerciseDto>>(
            cancellationToken: cancellationToken);

        return exercises?.Count ?? 0;
    }

    public async Task<string> GetExerciseCorrectAnswerAsync(Guid exerciseId, CancellationToken cancellationToken = default)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync(
            $"/api/v1/exercises/{exerciseId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new NotFoundException("Exercise", exerciseId);

        var exercise = await response.Content.ReadFromJsonAsync<ExerciseDetailDto>(
            cancellationToken: cancellationToken);

        return exercise?.CorrectAnswer
            ?? throw new NotFoundException("Exercise correct answer", exerciseId);
    }

    public async Task<int> GetExerciseXpRewardAsync(Guid exerciseId, CancellationToken cancellationToken = default)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync(
            $"/api/v1/exercises/{exerciseId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return 0;

        var exercise = await response.Content.ReadFromJsonAsync<ExerciseDetailDto>(
            cancellationToken: cancellationToken);

        return exercise?.XpReward ?? 0;
    }

    private record LessonDto(Guid Id, VideoDto? Video);
    private record VideoDto(int DurationSeconds);
    private record ExerciseDto(Guid Id);
    private record ExerciseDetailDto(Guid Id, string CorrectAnswer, int XpReward);
}