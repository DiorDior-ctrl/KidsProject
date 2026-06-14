using FluentValidation;
using Hangfire;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NotificationService.API.Jobs;
using NotificationService.API.Middleware;
using NotificationService.Application.Repositories.Interfaces;
using NotificationService.Application.Services;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Infrastructure.Data;
using NotificationService.Infrastructure.ExternalServices;
using NotificationService.Infrastructure.Messaging;
using NotificationService.Infrastructure.Repositories;
using Scalar.AspNetCore;
using SharedKernel.Logging;
using System.IdentityModel.Tokens.Jwt;
using NotificationService.API.Jobs;

var builder = WebApplication.CreateBuilder(args);
builder.AddSerilog("NotificationService");
// CLEAR DEFAULT CLAIM MAPPINGS
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// DATETIME — PostgreSQL UTC fix
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// DATABASE
builder.Services.AddDbContext<NotificationServiceDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("NotificationServiceDb"),
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

// RATE LIMITING 
builder.Services.AddRateLimiter(options =>
{
    // Policy e përgjithshme — 100 request në minutë për çdo IP
    options.AddFixedWindowLimiter("GeneralPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

    // Policy për Auth endpoints — 10 login attempts në minutë
    options.AddFixedWindowLimiter("AuthPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });

    // Ktheje 429 Too Many Requests
    options.RejectionStatusCode = 429;
});

// REPOSITORIES
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();

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
        cfg.AutoStart = false;
        cfg.ConfigureEndpoints(context);
    });
});

// SERVICES
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INotificationService, NotificationService.Application.Services.NotificationService>();
builder.Services.AddScoped<INotificationTemplateService, NotificationTemplateService>();

// HANGFIRE 
builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(
            builder.Configuration.GetConnectionString("NotificationServiceDb"));
    });
});
builder.Services.AddHangfireServer();

// CONTROLLERS + OPENAPI
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddOpenApi();
// HEALTH CHECK
builder.Services.AddHealthChecks();


var app = builder.Build();

// AUTO MIGRATION
if (app.Environment.IsDevelopment())
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationServiceDbContext>();
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
        options.Title = "KidsProject — NotificationService API";
        options.Theme = ScalarTheme.Purple;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
// HANGFIRE Dashboard
app.UseHangfireDashboard("/hangfire");
// Regjistro recurring job — çdo 5 minuta
RecurringJob.AddOrUpdate<ProcessPendingNotificationsJob>(
    "process-pending-notifications",
    job => job.ExecuteAsync(),
    "*/5 * * * *");
app.UseRateLimiter();
app.MapControllers();
app.MapHealthChecks("/health");

await app.RunAsync();