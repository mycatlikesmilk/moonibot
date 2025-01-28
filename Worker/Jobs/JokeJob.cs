using Application.Features.SendJoke;
using Domain.Enums;
using MediatR;
using Quartz;

namespace Worker.Jobs;

public class JokeJob(IMediator mediator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        mediator.Send(new SendJokeCommand(JokeType.Random));
    }
}