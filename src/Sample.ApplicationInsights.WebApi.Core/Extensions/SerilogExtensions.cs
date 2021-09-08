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
            var config = TelemetryConfiguration.CreateDefault();
            config.InstrumentationKey = configuration.GetSection("APPINSIGHTS_INSTRUMENTATIONKEY").Value;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("ApplicationName", $"API Application Insights - {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}")
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
                .WriteTo.ApplicationInsights(config, TelemetryConverter.Traces)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}
