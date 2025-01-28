using Domain.Enums;
using MediatR;

namespace Application.Features.SendJoke;

public record SendJokeCommand(JokeType JokeType) : IRequest<bool>;