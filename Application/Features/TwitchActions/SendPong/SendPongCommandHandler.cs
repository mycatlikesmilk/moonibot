using Application.Services.ExternalServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.TwitchActions.SendPong;

public class SendPongCommandHandler(
    ITwitchService twitchService,
    ILogger<SendPongCommandHandler> logger)
    : IRequestHandler<SendPongCommand, bool>
{
    public Task<bool> Handle(SendPongCommand request, CancellationToken cancellationToken)
    {
        twitchService.Client.SendMessage(twitchService.ConnectedChannel, "pong");
        logger.LogInformation("Pong Command Executed");
        return Task.FromResult(true);
    }
}