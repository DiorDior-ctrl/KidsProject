using System.Text.Json;
using Anthropic.SDK;
using Anthropic.SDK.Messaging;
using LessonService.Application.DTOs.Responses;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Application.Services.Interfaces;
using LessonService.Domain.Enums;
using LessonService.Domain.Exceptions;
using LessonService.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LessonService.Infrastructure.ExternalServices;

public class AnthropicExerciseService : IAiExerciseService
{
    private readonly AnthropicClient _client;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly ILogger<AnthropicExerciseService> _logger;

    public AnthropicExerciseService(
        IConfiguration configuration,
        IExerciseRepository exerciseRepository,
        ILessonRepository lessonRepository,
        ILogger<AnthropicExerciseService> logger)
    {
        var apiKey = configuration["Anthropic:ApiKey"]
            ?? throw new InvalidOperationException("Anthropic API key mungon.");

        _client = new AnthropicClient(apiKey);
        _exerciseRepository = exerciseRepository;
        _lessonRepository = lessonRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ExerciseResponse>> GenerateExercisesAsync(
        Guid lessonId,
        string lessonTitle,
        string topic,
        int count,
        ExerciseType type,
        CancellationToken cancellationToken = default)
    {
        var lesson = await _lessonRepository.GetByIdAsync(lessonId, cancellationToken)
            ?? throw new NotFoundException("Lesson", lessonId);

        var prompt = BuildPrompt(lessonTitle, topic, count, type);

        _logger.LogInformation("Duke gjeneruar {Count} ushtrime AI për leksionin {LessonId}", count, lessonId);

        var messages = new List<Message>
        {
            new Message
            {
                Role = RoleType.User,
                Content = new List<ContentBase>
                {
                    new TextContent { Text = prompt }
                }
            }
        };

        var parameters = new MessageParameters
        {
            Messages = messages,
            Model = "claude-haiku-4-5-20251001",
            MaxTokens = 2000,
            System = new List<SystemMessage>
            {
                new SystemMessage("Ti je një mësues i gjuhës shqipe që krijon ushtrime edukative për fëmijë moshës 4-12 vjeç. Gjithmonë kthe përgjigjen në format JSON të vlefshëm.")
            }
        };

        var response = await _client.Messages.GetClaudeMessageAsync(parameters, cancellationToken);

        var jsonContent = response.Content.OfType<TextContent>().FirstOrDefault()?.Text ?? "[]";

        jsonContent = jsonContent
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();

        var generatedExercises = JsonSerializer.Deserialize<List<GeneratedExercise>>(
            jsonContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? new List<GeneratedExercise>();

        var exercises = new List<ExerciseResponse>();
        var orderIndex = 1;

        foreach (var generated in generatedExercises)
        {
            var exercise = Exercise.Create(
                lessonId,
                type,
                orderIndex++,
                JsonSerializer.Serialize(generated.Content),
                generated.CorrectAnswer,
                xpReward: 5);

            await _exerciseRepository.AddAsync(exercise, cancellationToken);

            exercises.Add(new ExerciseResponse(
                exercise.Id,
                exercise.LessonId,
                exercise.Type,
                exercise.OrderIndex,
                exercise.ContentJson,
                exercise.XpReward,
                exercise.CorrectAnswer));
        }

        _logger.LogInformation("{Count} ushtrime AI u gjeneruan dhe u ruajtën", exercises.Count);

        return exercises;
    }

    private static string BuildPrompt(string lessonTitle, string topic, int count, ExerciseType type)
    {
        var typeDescription = type switch
        {
            ExerciseType.ListenAndChoose => "dëgjim dhe zgjedhje",
            ExerciseType.MatchWordsToImages => "lidhje fjalësh me imazhe",
            ExerciseType.FillInTheLetters => "plotëso germën që mungon",
            ExerciseType.CompleteSentence => "plotëso fjalinë",
            _ => "ushtrim i përgjithshëm"
        };

        return $"Krijo {count} ushtrime të tipit \"{typeDescription}\" për leksionin \"{lessonTitle}\" me temë \"{topic}\". " +
               $"Ushtrimet duhet të jenë të përshtatshme për fëmijë shqiptarë moshës 4-12 vjeç. " +
               $"Kthe VETËM një array JSON pa asnjë tekst tjetër, në këtë format: " +
               $"[{{\"content\": {{\"question\": \"Pyetja\", \"options\": [\"op1\", \"op2\", \"op3\", \"op4\"]}}, \"correctAnswer\": \"përgjigja e saktë\"}}]";
    }

    private record GeneratedExercise(
        JsonElement Content,
        string CorrectAnswer);
}