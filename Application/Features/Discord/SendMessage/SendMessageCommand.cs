using MediatR;

namespace Application.Features.Discord.SendMessage;

public record SendMessageCommand(
    ulong ChannelId,
    string Message
    ) : IRequest;