using Discord;
using MediatR;

namespace Application.Features.Discord.SendFile;

public record SendFileCommand(
    ulong ChannelId,
    FileAttachment FileAttachment,
    string Message
) : IRequest;