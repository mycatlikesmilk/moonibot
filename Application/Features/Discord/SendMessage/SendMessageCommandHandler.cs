using Application.Services.ExternalServices;
using Discord;
using MediatR;

namespace Application.Features.Discord.SendMessage;

public class SendMessageCommandHandler(IDiscordService discordService) : IRequestHandler<SendMessageCommand>
{
    public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = (IMessageChannel)discordService.DiscordClient.GetChannel(request.ChannelId);
        await channel.SendMessageAsync(text: request.Message);
    }
}