using Application.Services.ExternalServices;
using Discord;
using MediatR;

namespace Application.Features.Discord.SendFile;

public class SendFileCommandHandler(IDiscordService discordService) : IRequestHandler<SendFileCommand>
{
    public async  Task Handle(SendFileCommand request, CancellationToken cancellationToken)
    {
        var channel = (ITextChannel)discordService.DiscordClient.GetChannel(request.ChannelId);
        await channel.SendFileAsync(request.FileAttachment, request.Message);
    }
}