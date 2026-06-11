using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using UserService.API.Middleware;
using UserService.Application.Repositories.Interfaces;
using UserService.Application.Services;
using UserService.Application.Services.Interfaces;
using UserService.Application.Validators;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.ExternalServices;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// CLEAR DEFAULT CLAIM MAPPINGS — lexo rolet saktë nga Keycloak
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// DATABASE
builder.Services.AddDbContext<UserServiceDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("UserServiceDb"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null);
        });

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging().EnableDetailedErrors();
});

// AUTHENTICATION — Keycloak
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = bool.Parse(
            builder.Configuration["Keycloak:RequireHttpsMetadata"] ?? "false");
    });

// AUTHORIZATION — RBAC
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("ParentOnly", policy =>
        policy.RequireRole("Parent"));

    options.AddPolicy("ChildOnly", policy =>
        policy.RequireRole("Child"));

    options.AddPolicy("ParentOrAdmin", policy =>
        policy.RequireRole("Parent", "Admin"));
});

// REPOSITORIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChildProfileRepository, ChildProfileRepository>();
builder.Services.AddScoped<IParentChildRelationRepository, ParentChildRelationRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();

// VALIDATORS
builder.Services.AddValidatorsFromAssemblyContaining<RegisterParentValidator>();

// SERVICES
builder.Services.AddHttpClient<IKeyCloakService, KeycloakService>();
builder.Services.AddScoped<IUserService, UserService.Application.Services.UserService>();
builder.Services.AddScoped<IChildProfileService, ChildProfileService>();

// CONTROLLERS + OPENAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// AUTO MIGRATION — vetëm në Development
if (app.Environment.IsDevelopment())
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
        await db.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Migration dështoi: {Message}", ex.Message);
    }

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "KidsProject — UserService API";
        options.Theme = ScalarTheme.Purple;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();