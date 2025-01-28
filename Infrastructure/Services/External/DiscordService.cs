using Application.Common.Constants;
using Application.Services.ExternalServices;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.External;

public class DiscordService(
    IConfiguration config,
    ILogger<DiscordService> logger)
    : IDiscordService
{
    private DiscordSocketClient _client;
    private bool _isClientReady;
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var token = config
            .GetSection("Discord:Credentials")
            .GetChildren()
            .FirstOrDefault(x => x["Name"] == "Moonibot")!["AccessToken"];

        var discordConfig = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All,
        };
        
        _client = new DiscordSocketClient(discordConfig);
        _client.Ready += HandleClientReady;
        _client.MessageReceived += HandleMessageReceived;
        
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await WaitDiscordReady();
    }

    private async Task HandleMessageReceived(SocketMessage arg)
    {
        throw new NotImplementedException();
    }

    private Task HandleClientReady()
    {
        _isClientReady = true;
        logger.LogInformation(LogMessages.DiscordServiceConnected);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.StopAsync();
        await _client.LogoutAsync();
    }

    private async Task WaitDiscordReady()
    {
        while (!_isClientReady)
        {
            await Task.Delay(3000);
        }
    }
}