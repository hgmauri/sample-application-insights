using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

namespace Sample.ApplicationInsights.WebApi.Core.Extensions
{
    public static class ApplicationInsightsExtensions
    {
        public static void AddLiveMetrisApplicationInsights(this TelemetryConfiguration appInsightsConfig)
        {
            var builder = appInsightsConfig.TelemetryProcessorChainBuilder;
            QuickPulseTelemetryProcessor quickPulseProcessor = null;
            builder.Use((next) =>
            {
                quickPulseProcessor = new QuickPulseTelemetryProcessor(next);
                return quickPulseProcessor;
            });
            builder.Build();

            var quickPulse = new QuickPulseTelemetryModule();
            quickPulse.Initialize(appInsightsConfig);
            quickPulse.RegisterTelemetryProcessor(quickPulseProcessor);
        }
    }
}
