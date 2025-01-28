using Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Serilog.Formatting;
using Worker.Common;
using Worker.Common.TextFormatters;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Worker;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        
        var shortSourceContextFormatter = new ShortSourceContextFormatter();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Quartz", LogEventLevel.Warning)
            .MinimumLevel.Override("StdSchedulerFactory", LogEventLevel.Warning)
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console(
                formatter: shortSourceContextFormatter)
            .WriteTo.File(
                path: "Logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                formatter: shortSourceContextFormatter)
            .WriteTo.Sink(new DiscordSink(
                webhookUrl: "https://discord.com/api/webhooks/1331758335300472852/0KuG5s9sisJ6ma1scQ4dPaqdxzyW0baPigMaMLEVVAtT68gOtYKEZaPa_RIJ8eQ2N9g7"))
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure();
        builder.Services.AddWorker();
        
        var host = builder.Build();
        
        await host.RunAsync();
    }
}