using Application.Features.SendJoke;
using Domain.Enums;
using MediatR;

namespace Application.Features.SendRandomJoke;

public class SendRandomJokeCommandHandler(IMediator mediator) : IRequestHandler<SendRandomJokeCommand, bool>
{
    public async Task<bool> Handle(SendRandomJokeCommand request, CancellationToken cancellationToken)
    {
        var random = new Random();
        var flag = false;
        while (!flag)
        {
            var rndVal = random.Next(0, 2);
            flag = await mediator.Send(new SendJokeCommand((JokeType)rndVal), cancellationToken);
        }

        return true;
    }
}