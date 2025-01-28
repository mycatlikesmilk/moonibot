using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace Worker.Common.TextFormatters;

public class ShortSourceContextFormatter : ITextFormatter
{
    private readonly MessageTemplateTextFormatter _formatter =
        new("[{Timestamp:HH:mm:ss} {Level:u3}] [{ShortSourceContext}] {Message:lj}{NewLine}{Exception}");

    public void Format(LogEvent logEvent, TextWriter output)
    {
        if (logEvent.Properties.TryGetValue("SourceContext", out var sourceContextValue))
        {
            var sourceContext = sourceContextValue.ToString().Trim('"');
            var shortName = sourceContext.Split('.').Last();
            logEvent.AddOrUpdateProperty(new LogEventProperty("ShortSourceContext", new ScalarValue(shortName)));
        }

        _formatter.Format(logEvent, output);
    }
}