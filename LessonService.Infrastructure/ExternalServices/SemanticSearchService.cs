using System.Text.Json;
using Anthropic.SDK;
using Anthropic.SDK.Messaging;
using LessonService.Application.DTOs.Responses;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Application.Services.Interfaces;
using LessonService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace LessonService.Infrastructure.ExternalServices;

public class SemanticSearchService : ISemanticSearchService
{
    private readonly LessonServiceDbContext _context;
    private readonly AnthropicClient _client;
    private readonly ILogger<SemanticSearchService> _logger;

    public SemanticSearchService(
        LessonServiceDbContext context,
        IConfiguration configuration,
        ILogger<SemanticSearchService> logger)
    {
        _context = context;
        _logger = logger;

        var apiKey = configuration["Anthropic:ApiKey"] ?? string.Empty;
        _client = new AnthropicClient(apiKey);
    }

    public async Task<IEnumerable<LessonResponse>> SearchAsync(
        string query,
        int topK = 5,
        CancellationToken cancellationToken = default)
    {
        var queryEmbedding = await GetEmbeddingAsync(query, cancellationToken);

        if (queryEmbedding == null)
        {
            _logger.LogWarning("Embedding nuk u gjenerua për query: {Query}", query);
            return Enumerable.Empty<LessonResponse>();
        }

        var vector = new Vector(queryEmbedding);

        var lessons = await _context.Lessons
            .Where(l => l.Embedding != null)
            .OrderBy(l => l.Embedding!.CosineDistance(vector))
            .Take(topK)
            .ToListAsync(cancellationToken);

        return lessons.Select(l => new LessonResponse(
            l.Id, l.ModuleId, l.Title,
            l.OrderIndex, l.XpReward, l.HasVideo, l.CreatedAt));
    }

    public async Task IndexLessonAsync(
        Guid lessonId,
        string title,
        CancellationToken cancellationToken = default)
    {
        var lesson = await _context.Lessons
            .FirstOrDefaultAsync(l => l.Id == lessonId, cancellationToken);

        if (lesson == null) return;

        var embedding = await GetEmbeddingAsync(title, cancellationToken);
        if (embedding == null) return;

        lesson.SetEmbedding(embedding);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Leksioni {LessonId} u indeksua për semantic search", lessonId);
    }

    private async Task<float[]?> GetEmbeddingAsync(string text, CancellationToken cancellationToken)
    {
        try
        {
            // Përdorim Claude për të gjeneruar embedding të thjeshtë
            // Në production do të përdornim OpenAI embeddings ose modele të dedikuara
            var messages = new List<Message>
            {
                new Message
                {
                    Role = RoleType.User,
                    Content = new List<ContentBase>
                    {
                        new TextContent
                        {
                            Text = $"Kthe VETËM një array JSON me 1536 numra float midis -1 dhe 1 që reprezenton embedding-un e tekstit: \"{text}\". Asnjë tekst tjetër."
                        }
                    }
                }
            };

            var parameters = new MessageParameters
            {
                Messages = messages,
                Model = "claude-haiku-4-5-20251001",
                MaxTokens = 4096,
                System = new List<SystemMessage>
                {
                    new SystemMessage("Kthe VETËM array JSON me numra float. Asnjë tekst tjetër.")
                }
            };

            var response = await _client.Messages.GetClaudeMessageAsync(parameters, cancellationToken);
            var jsonContent = response.Content.OfType<TextContent>().FirstOrDefault()?.Text ?? "[]";

            jsonContent = jsonContent.Replace("```json", "").Replace("```", "").Trim();

            return JsonSerializer.Deserialize<float[]>(jsonContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Embedding gjenerimi dështoi për: {Text}", text);
            return null;
        }
    }
}