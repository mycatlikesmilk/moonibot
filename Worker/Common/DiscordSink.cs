using Serilog.Core;
using Serilog.Events;

namespace Worker.Common;

public class DiscordSink(string webhookUrl) : ILogEventSink
{
    private readonly HttpClient _httpClient = new();

    public void Emit(LogEvent logEvent)
    {
        logEvent.Properties.TryGetValue("SourceContext", out var source);
        
        var message = new
        {
            content = $"`[{logEvent.Timestamp:HH:mm:ss} {logEvent.Level.ToString()[..3].ToUpper()}] [{source.ToString().Trim('"').Split('.').Last()}]  {logEvent.RenderMessage()}`"
        };
        
        var json = System.Text.Json.JsonSerializer.Serialize(message);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        _ = _httpClient.PostAsync(webhookUrl, content).GetAwaiter().GetResult();
    }
}