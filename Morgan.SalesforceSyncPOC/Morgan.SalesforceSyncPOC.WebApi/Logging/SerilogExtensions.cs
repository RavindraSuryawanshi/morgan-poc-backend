using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

namespace Morgan.SalesforceSyncPOC.WebApi.Logging
{
    /// <summary>
    /// Provides extension methods for configuring Serilog logging
    /// and Application Insights integration for the application.
    /// </summary>
    public static class SerilogExtensions
    {
        public static void ConfigureSerilog(
            this WebApplicationBuilder builder)
        {
            var applicationName =
                builder.Configuration["ApplicationName"];

            var aiConnectionString =
                builder.Configuration["ApplicationInsightsConnectionString"];

            var telemetryConfiguration =
                TelemetryConfiguration.CreateDefault();

            telemetryConfiguration.ConnectionString =
                aiConnectionString;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override(
                    "Microsoft",
                    LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty(
                    "ApplicationName",
                    applicationName)
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(
                    telemetryConfiguration,
                    TelemetryConverter.Traces)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}