using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Exceptions;

namespace UserService.Infrastructure.ExternalServices;

public class KeycloakService : IKeyCloakService
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakSettings _settings;
    private readonly ILogger<KeycloakService> _logger;

    public KeycloakService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<KeycloakService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _settings = new KeycloakSettings
        {
            AdminUrl = configuration["Keycloak:AdminUrl"] ?? string.Empty,
            Realm = configuration["Keycloak:Realm"] ?? string.Empty,
            ClientId = configuration["Keycloak:ClientId"] ?? string.Empty,
            ClientSecret = configuration["Keycloak:ClientSecret"] ?? string.Empty,
            AdminUsername = configuration["Keycloak:AdminUsername"] ?? string.Empty,
            AdminPassword = configuration["Keycloak:AdminPassword"] ?? string.Empty
        };
    }

    // Merr Admin Token nga Keycloak për të bërë operacione administrative
    private async Task<string> GetAdminTokenAsync(CancellationToken cancellationToken)
    {
        var url = $"{_settings.AdminUrl}/realms/master/protocol/openid-connect/token";

        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = "admin-cli",
            ["username"] = _settings.AdminUsername,
            ["password"] = _settings.AdminPassword
        });

        var response = await _httpClient.PostAsync(url, body, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Keycloak admin token dështoi: {Status} {Error}",
                response.StatusCode, error);
            throw new BusinessException("Keycloak authentication dështoi.");
        }

        var content = await response.Content.ReadFromJsonAsync<JsonElement>(
            cancellationToken: cancellationToken);
        return content.GetProperty("access_token").GetString()
            ?? throw new BusinessException("Token nuk u mor nga Keycloak.");
    }

    public async Task<string> RegisterUserAsync(
        string email,
        string password,
        string role,
        CancellationToken cancellationToken = default)
    {
        var adminToken = await GetAdminTokenAsync(cancellationToken);

        var url = $"{_settings.AdminUrl}/admin/realms/{_settings.Realm}/users";

        var userBody = new
        {
            username = email,
            email = email,
            enabled = true,
            emailVerified = true,
            requiredActions = Array.Empty<string>(),  // ← shto këtë
            credentials = new[]
    {
        new
        {
            type = "password",
            value = password,
            temporary = false  // ← sigurohu që është false
        }
    },
            attributes = new Dictionary<string, string[]>
            {
                ["role"] = new[] { role }
            }
        };

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _httpClient.PostAsJsonAsync(url, userBody, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Keycloak regjistrim dështoi: {Error}", error);
            throw new BusinessException("Regjistrimi dështoi. Provoni përsëri.");
        }

        // Keycloak e kthen ID-në në Location header
        var location = response.Headers.Location?.ToString()
            ?? throw new BusinessException("Keycloak nuk ktheu ID të userit.");

        var keycloakId = location.Split('/').Last();

        _logger.LogInformation("User u regjistrua në Keycloak: {KeycloakId}", keycloakId);

        return keycloakId;
    }

    public async Task<string> GetTokenAsync(
    string email,
    string password,
    CancellationToken cancellationToken = default)
    {
        var url = $"{_settings.AdminUrl}/realms/{_settings.Realm}/protocol/openid-connect/token";

        var body = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = _settings.ClientId,
            ["client_secret"] = _settings.ClientSecret,
            ["username"] = email,
            ["password"] = password
        });

        _httpClient.DefaultRequestHeaders.Authorization = null;

        var response = await _httpClient.PostAsync(url, body, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Login dështoi për: {Email}", email);
            throw new BusinessException("Email ose fjalëkalim i gabuar.");
        }

        var content = await response.Content.ReadFromJsonAsync<JsonElement>(
            cancellationToken: cancellationToken);
        return content.GetProperty("access_token").GetString()
            ?? throw new BusinessException("Token nuk u mor.");
    }

    public async Task DeleteUserAsync(
        string keycloakId,
        CancellationToken cancellationToken = default)
    {
        var adminToken = await GetAdminTokenAsync(cancellationToken);

        var url = $"{_settings.AdminUrl}/admin/realms/{_settings.Realm}/users/{keycloakId}";

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _httpClient.DeleteAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Keycloak fshirje dështoi për: {KeycloakId}", keycloakId);
            throw new BusinessException("Fshirja e userit dështoi.");
        }

        _logger.LogInformation("User u fshi nga Keycloak: {KeycloakId}", keycloakId);
    }
}