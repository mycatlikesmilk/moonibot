using Application.Common.Constants;
using Application.Features.TwitchActions.SendPong;
using Application.Services.ExternalServices;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Infrastructure.Services.External;

public class TwitchService(
    IConfiguration config,
    ILogger<TwitchService> logger,
    IMediator mediator)
    : ITwitchService, IHostedService
{
    public required TwitchClient Client { get; set; }
    public required string ConnectedChannel { get; set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Twitch service");
        Client = new TwitchClient();
        
        var accessToken = config
            .GetSection("Twitch:Credentials")
            .GetChildren()
            .FirstOrDefault(x => x["Name"] == "mooni_bot")!["AccessToken"];
        
        ConnectedChannel = config
            .GetSection("Twitch:Credentials")
            .GetChildren()
            .FirstOrDefault(x => x["Name"] == "mooni_bot")!["ChannelName"]!;

        var credentials = new ConnectionCredentials("mooni_bot", accessToken);
        
        Client.OnConnected += ClientConnected;
        Client.OnMessageReceived += HandleMessage;
        
        Client.Initialize(credentials, ConnectedChannel);
        Client.Connect();
        logger.LogInformation("Twitch service started successfully");
        return Task.CompletedTask;
    }

    private void HandleMessage(object? sender, OnMessageReceivedArgs e)
    {
        if (e.ChatMessage.Message == "!ping")
            mediator.Send(new SendPongCommand());
    }

    private void ClientConnected(object? sender, OnConnectedArgs e)
    {
        logger.LogInformation(LogMessages.TwitchServiceConnected);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Twitch service");
        Client.Disconnect();
        return Task.CompletedTask;
    }
}