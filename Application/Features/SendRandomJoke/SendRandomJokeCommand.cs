using MediatR;

namespace Application.Features.SendRandomJoke;

public record SendRandomJokeCommand : IRequest<bool>;