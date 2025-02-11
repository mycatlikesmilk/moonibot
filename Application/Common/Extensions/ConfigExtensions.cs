using Microsoft.Extensions.Configuration;

namespace Application.Common.Extensions;

public static class ConfigExtensions
{
    public static string GetDiscordToken(this IConfiguration configuration, string name)
    {
        var credentials = configuration
            .GetSection("Discord:Credentials")
            .GetChildren()
            .FirstOrDefault(x => x["Name"] == name);

        return credentials?["Token"] ?? String.Empty;
    }

    public static ulong GetDiscordChannel(this IConfiguration configuration, string channelName)
    {
        var channel = configuration
            .GetSection("Discord:Channels")
            .GetChildren()
            .FirstOrDefault(x => x["Name"] == channelName);

        return ulong.TryParse(channel["Id"], out var output) ? output : 0;
    }

    public static string GetTelegramToken(this IConfiguration configuration, string name)
    {
        var credentials = configuration
            .GetSection("Telegram:Credentials")
            .GetChildren()
            .FirstOrDefault(x => x["Name"] == name);

        return credentials?["Token"] ?? String.Empty;
    }
}