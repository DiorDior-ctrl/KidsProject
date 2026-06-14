using FluentValidation;
using GamificationService.API.Middleware;
using GamificationService.Application.Repositories.Interfaces;
using GamificationService.Application.Services;
using GamificationService.Application.Services.Interfaces;
using GamificationService.Infrastructure.Data;
using GamificationService.Infrastructure.Repositories;
using GamificationService.Infrastructure.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// CLEAR DEFAULT CLAIM MAPPINGS
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// DATETIME — PostgreSQL UTC fix
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// DATABASE
builder.Services.AddDbContext<GamificationServiceDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("GamificationServiceDb"),
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
builder.Services.AddScoped<IUserXpRepository, UserXpRepository>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<IUserBadgeRepository, UserBadgeRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();

// MASSTRANSIT — RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LessonCompletedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

// SERVICES
builder.Services.AddScoped<IGamificationService, GamificationService.Application.Services.GamificationService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

// CONTROLLERS + OPENAPI
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// AUTO MIGRATION
if (app.Environment.IsDevelopment())
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GamificationServiceDbContext>();
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
        options.Title = "KidsProject — GamificationService API";
        options.Theme = ScalarTheme.Purple;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();