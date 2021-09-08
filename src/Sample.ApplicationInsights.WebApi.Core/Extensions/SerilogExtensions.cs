using System;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Filters;

namespace Sample.ApplicationInsights.WebApi.Core.Extensions
{
    public static class SerilogExtensions
    {
        public static void AddSerilogApi(IConfiguration configuration)
        {
            var appInsightsConfig = TelemetryConfiguration.CreateDefault();
            appInsightsConfig.InstrumentationKey = configuration.GetSection("APPINSIGHTS_INSTRUMENTATIONKEY").Value;
            appInsightsConfig.AddLiveMetrisApplicationInsights();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("ApplicationName", $"API Application Insights - {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}")
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Hosting.Diagnostics"))
                .Filter.ByExcluding(Matching.FromSource("Serilog.AspNetCore.RequestLoggingMiddleware"))
                .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
                .WriteTo.ApplicationInsights(appInsightsConfig, TelemetryConverter.Traces)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
