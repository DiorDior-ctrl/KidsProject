using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace SharedKernel.Logging;

public static class SerilogConfiguration
{
    public static WebApplicationBuilder AddSerilog(
        this WebApplicationBuilder builder,
        string serviceName)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ServiceName", serviceName)
                .Enrich.WithProperty("Environment",
                    context.HostingEnvironment.EnvironmentName)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{ServiceName}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.GrafanaLoki(
                    context.Configuration["Loki:Url"] ?? "http://localhost:3100",
                    labels: new[]
                    {
                        new Serilog.Sinks.Grafana.Loki.LokiLabel
                        {
                            Key = "service",
                            Value = serviceName
                        },
                        new Serilog.Sinks.Grafana.Loki.LokiLabel
                        {
                            Key = "environment",
                            Value = context.HostingEnvironment.EnvironmentName
                        }
                    });
        });

        return builder;
    }
}