using MediatR;

namespace Application.Features.TwitchActions.SendPong;

public record SendPongCommand : IRequest<bool>;