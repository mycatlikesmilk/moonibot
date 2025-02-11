using System.Text;
using Application.Common.Extensions;
using Application.Features.Discord.SendFile;
using Application.Features.Discord.SendFiles;
using Application.Features.Discord.SendMessage;
using Application.Services.ExternalServices;
using Discord;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Services.External;

public class TelegramService(
    IConfiguration configuration,
    IMediator mediator,
    ILogger<TelegramService> logger
) : ITelegramService, IHostedService
{
    private const string Footer = "\n\n*[Официальный Telegram-канал Mooniverse](<https://t.me/mnvrse>)*";

    private TelegramBotClient _client = null!;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Telegram service");

        var accessToken = configuration.GetTelegramToken("Moonibot");

        _client = new TelegramBotClient(accessToken);

        var offset = 0;

        logger.LogInformation("Telegram service started successfully");
        while (cancellationToken.IsCancellationRequested == false)
        {
            var updates = await _client.GetUpdates(
                offset: offset,
                allowedUpdates: [UpdateType.ChannelPost],
                cancellationToken: cancellationToken);

            foreach (var update in updates)
            {
                offset = update.Id + 1 > offset ? update.Id + 1 : offset;
                await HandleUpdate(update);
            }
        }
        
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Telegram service");
        await _client.Close(cancellationToken: cancellationToken);
    }

    private async Task HandleUpdate(Update update)
    {
        var message = "";
        var channelId = configuration.GetDiscordChannel("telegram");

        if (update.Type != UpdateType.ChannelPost)
            return;

        List<string> files = new List<string>();

        if (update.ChannelPost!.Text != null)
            message += update.ChannelPost!.Text;
        if (update.ChannelPost!.Poll != null)
            message += GeneratePoll(update.ChannelPost);
        
        if (update.ChannelPost!.Photo != null)
            files.Add(update.ChannelPost.Photo.Select(x => x.FileId).Last());
        if (update.ChannelPost!.Document != null)
            files.Add(update.ChannelPost.Document.FileId);
        if (update.ChannelPost!.Video != null)
            files.Add(update.ChannelPost.Video.FileId);
        if (update.ChannelPost!.Animation != null)
            files.Add(update.ChannelPost.Animation.FileId);
        if (update.ChannelPost!.Audio != null)
            files.Add(update.ChannelPost.Audio.FileId);
        if (update.ChannelPost!.VideoNote != null)
            files.Add(update.ChannelPost.VideoNote.FileId);
        if (update.ChannelPost!.Sticker != null)
            files.Add(update.ChannelPost.Sticker.FileId);
        if (update.ChannelPost!.Voice != null)
            files.Add(update.ChannelPost.Voice.FileId);

        files = files.Distinct().ToList();

        if (!string.IsNullOrWhiteSpace(update.ChannelPost!.Text) || update.ChannelPost.Poll != null)
        {
            await mediator.Send(new SendMessageCommand(channelId, message + Footer));
        }
        else
        {
            message = update.ChannelPost.Caption;
            
            var fileCaption = string.IsNullOrWhiteSpace(message) ? "" : message + Footer;
            
            foreach (var file in files)
            {
                using var memStream = new MemoryStream();
                var fileData = await _client.GetFile(file);
                var fileName = fileData.FilePath.Split('/').Last();
                try
                {
                    await _client.DownloadFile(fileData.FilePath!, memStream);
                    await mediator.Send(new SendFileCommand(channelId, new FileAttachment(memStream, fileName), fileCaption));
                }
                catch
                {
                    // Ignored
                }
            }
            
        }
    }

    private string GeneratePoll(Message channelPost)
    {
        var builder = new StringBuilder();

        builder.Append($"[Опрос] **{channelPost.Poll!.Question}**");
        builder.Append($"\n\n");
        builder.Append($"{string.Join('\n', channelPost.Poll.Options.Select(x => $"- {x.Text}"))}");
        builder.Append($"\n");
        builder.Append($"__[Голосование доступно только в Telegram-канале](<https://t.me/c/{channelPost.Chat.Id.ToString().Substring(4)}/{channelPost.MessageId}>)__");

        return builder.ToString();
    }
}