using FluentValidation;
using LessonService.API.Middleware;
using LessonService.Application.Repositories.Interfaces;
using LessonService.Application.Services;
using LessonService.Application.Services.Interfaces;
using LessonService.Application.Validators;
using LessonService.Infrastructure.Data;
using LessonService.Infrastructure.ExternalServices;
using LessonService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// CLEAR DEFAULT CLAIM MAPPINGS
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// DATETIME — PostgreSQL UTC fix
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// DATABASE
builder.Services.AddDbContext<LessonServiceDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("LessonServiceDb"),
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
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<ILessonVideoRepository, LessonVideoRepository>();

// REDIS CACHE
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";
    options.InstanceName = "LessonService:";
});
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// VALIDATORS
builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseValidator>();

// SERVICES
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<ILessonService, LessonService.Application.Services.LessonService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

// CONTROLLERS + OPENAPI
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => new { Field = e.Key, Errors = e.Value?.Errors.Select(x => x.ErrorMessage) });

            return new BadRequestObjectResult(errors);
        };
    });

builder.Services.AddOpenApi();

var app = builder.Build();

// AUTO MIGRATION
if (app.Environment.IsDevelopment())
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LessonServiceDbContext>();
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
        options.Title = "KidsProject — LessonService API";
        options.Theme = ScalarTheme.Purple;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();