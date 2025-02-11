using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Features.SendJoke;
using Application.Services.ExternalServices;
using Discord;
using Discord.WebSocket;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.External;

public class CyberboomationService(
    IMediator mediator,
    ILogger<CyberboomationService> logger,
    IConfiguration configuration
) : ICyberboomationService
{
    private DiscordSocketClient _client;
    private bool _isClientReady;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Cyberboomation service");

        var token = configuration.GetDiscordToken("Cyberboomation");

        var config = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All
        };
        
        _client = new DiscordSocketClient(config);

        _client.Ready += OnClientReady;
        _client.MessageReceived += HandleReceiveMessageAsync;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await WaitDiscordReady();
        
        logger.LogInformation("Cyberboomation service started successfully");
    }

    public async Task<bool> SendMessageAsync(ulong channelId, string message)
    {
        var channel = (IMessageChannel)_client.GetChannel(channelId);
        await channel.SendMessageAsync(text: message);
        return true;
    }

    public async Task HandleReceiveMessageAsync(SocketMessage arg)
    {
        var subChannel = configuration.GetDiscordChannel("sub");
        if (subChannel == arg.Channel.Id && !(arg.Author.IsBot || arg.Author.IsWebhook))
        {
            switch (arg.Content)
            {
                case "!анекб":
                    await mediator.Send(new SendJokeCommand(JokeType.JokeB));
                    break;
                case "!анек":
                    await mediator.Send(new SendJokeCommand(JokeType.Joke));
                    break;
                case "!анек про улитку":
                    await mediator.Send(new SendJokeCommand(JokeType.Snail));
                    break;
            }
        }
    }

    public Task HandleCommandAsync(SocketSlashCommand command)
    {
        throw new NotImplementedException();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Cyberboomation service");
        await _client.LogoutAsync();
        await _client.StopAsync();
    }

    private async Task WaitDiscordReady()
    {
        while (!_isClientReady)
        {
            await Task.Delay(3000);
        }
    }

    private async Task OnClientReady()
    {
        _isClientReady = true;
        logger.LogInformation(LogMessages.CyberboomationServiceConnected);
    }
}