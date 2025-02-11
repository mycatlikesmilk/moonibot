using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Services.ExternalServices;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.External;

public class DiscordService(
    IConfiguration configuration,
    ILogger<DiscordService> logger)
    : IDiscordService
{
    public DiscordSocketClient DiscordClient { get; set; }
    
    private bool _isClientReady;
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Main Discord service");
        
        var token = configuration.GetDiscordToken("Moonibot");

        var discordConfig = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All,
        };
        
        DiscordClient = new DiscordSocketClient(discordConfig);
        
        DiscordClient.Ready += HandleDiscordClientReady;
        DiscordClient.MessageReceived += HandleMessageReceived;
        
        await DiscordClient.LoginAsync(TokenType.Bot, token);
        await DiscordClient.StartAsync();
        await WaitDiscordReady();
        
        logger.LogInformation("Main Discord service started successfully");
    }

    private async Task HandleMessageReceived(SocketMessage arg)
    {
        throw new NotImplementedException();
    }

    private Task HandleDiscordClientReady()
    {
        _isClientReady = true;
        logger.LogInformation(LogMessages.DiscordServiceConnected);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Main Discord service");
        await DiscordClient.StopAsync();
        await DiscordClient.LogoutAsync();
    }

    private async Task WaitDiscordReady()
    {
        while (!_isClientReady)
        {
            await Task.Delay(3000);
        }
    }
}