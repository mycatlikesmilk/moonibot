using Application.Services.ExternalServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Services.External;

public class TelegramService(IConfiguration configuration) : ITelegramService, IHostedService
{
    private TelegramBotClient _client;
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var accessToken = configuration
            .GetSection("Telegram:Credentials")
            .GetChildren()
            .FirstOrDefault(x => x["Name"] == "Moonibot")!["Token"];
        
        _client = new TelegramBotClient(accessToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.Close(cancellationToken: cancellationToken);
    }

    public async Task<Update[]> GetUpdates(int offset)
    {
        return await _client.GetUpdates(offset, allowedUpdates: [UpdateType.ChannelPost]);
    }
}