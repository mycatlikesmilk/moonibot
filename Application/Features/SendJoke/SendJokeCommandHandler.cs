using Application.Common.Extensions;
using Application.Common.Helper;
using Application.Services;
using Application.Services.ExternalServices;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Features.SendJoke;

public class SendJokeCommandHandler(
    IMediator mediator,
    ICyberboomationService cyberboomationService,
    IConfiguration configuration,
    IHttpClientService httpClientService)
    : IRequestHandler<SendJokeCommand, bool>
{
    public async Task<bool> Handle(SendJokeCommand request, CancellationToken cancellationToken)
    {
        var message = request.JokeType switch
        {
            JokeType.Joke => await JokeHelper.GetJoke(httpClientService),
            JokeType.JokeB => await JokeHelper.GetBJoke(httpClientService),
            JokeType.Random => await JokeHelper.GetRandomJoke(httpClientService),
            JokeType.Snail => await JokeHelper.GetSnailJoke(),
            _ => ""
        };

        var channelId = configuration.GetDiscordChannel("sub");

        await cyberboomationService.SendMessageAsync(channelId, message);
        return true;
    }
}