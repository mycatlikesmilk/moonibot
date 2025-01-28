using Discord.WebSocket;
using Microsoft.Extensions.Hosting;

namespace Application.Services.ExternalServices;

public interface ICyberboomationService : IHostedService
{
    Task<bool> SendMessageAsync(ulong channelId, string message);
    Task HandleReceiveMessageAsync(SocketMessage arg);
    Task HandleCommandAsync(SocketSlashCommand command);
}