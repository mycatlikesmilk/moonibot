using Discord;
using MediatR;

namespace Application.Features.Discord.SendFiles;

public record SendFilesCommand(
    ulong ChannelId,
    FileAttachment[] FileAttachment,
    string Message
    ) : IRequest;