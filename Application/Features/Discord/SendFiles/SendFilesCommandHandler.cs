using Application.Services.ExternalServices;
using Discord;
using MediatR;

namespace Application.Features.Discord.SendFiles;

public class SendFilesCommandHandler(IDiscordService discordService)
    : IRequestHandler<SendFilesCommand>
{
    public async Task Handle(SendFilesCommand request, CancellationToken cancellationToken)
    {
        var channel = (ITextChannel)discordService.DiscordClient.GetChannel(request.ChannelId);
        await channel.SendFilesAsync(request.FileAttachment, request.Message);
    }
}