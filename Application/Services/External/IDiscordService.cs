using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace Application.Services.ExternalServices;

public interface IDiscordService : IHostedService
{
    DiscordSocketClient DiscordClient { get; set; }
}