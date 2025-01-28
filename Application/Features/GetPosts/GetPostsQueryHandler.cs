using System.Text;
using Application.Common.Extensions;
using Application.Services.ExternalServices;
using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Application.Features.GetPosts;

public class GetPostsQueryHandler(
    //ITelegramService telegramService,
    ICyberboomationService cyberboomationService,
    IConfiguration configuration,
    IMediator mediator)
    : IRequestHandler<GetPostsQuery, List<Update>>
{
    private const string PollText =
        @"[Опрос] **{0}**\n\n{1}\n__[Голосование доступно только в Telegram-канале](<https://t.me/c/{2}/{3}>__";

    private const string Footer =
        @"\n\n*[Официальный Telegram-канал Mooniverse](<https://t.me/mnvrse>)*";
    
    public async Task<List<Update>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        // var updates = await telegramService.GetUpdates(request.Offset);
        //
        // foreach (var update in updates)
        // {
        //     await HandleUpdate(update);
        // }

        return null;
    }

    private async Task HandleUpdate(Update update)
    {
        if (update.Type != UpdateType.ChannelPost)
            return;

        var telegramChannelId = configuration.GetDiscordChannel("telegram");

        List<string> files = new List<string>();
        StringBuilder messageText = new StringBuilder();
        
        if (update.ChannelPost.Photo != null)
            files.Add(update.ChannelPost.Photo.Select(x => x.FileId).Last());
        if (update.ChannelPost.Document != null)
            files.Add(update.ChannelPost.Document.FileId);
        if (update.ChannelPost.Video != null)
            files.Add(update.ChannelPost.Video.FileId);
        if (update.ChannelPost.Animation != null)
            files.Add(update.ChannelPost.Animation.FileId);
        if (update.ChannelPost.Audio != null)
            files.Add(update.ChannelPost.Audio.FileId);
        if (update.ChannelPost.VideoNote != null)
            files.Add(update.ChannelPost.VideoNote.FileId);
        if (update.ChannelPost.Sticker != null)
            files.Add(update.ChannelPost.Sticker.FileId);
        if (update.ChannelPost.Voice != null)
            files.Add((update.ChannelPost.Voice.FileId));
        if (update.ChannelPost.Poll != null)
        {
            messageText.Append("");
        }
        if (!string.IsNullOrWhiteSpace(update.ChannelPost?.Text))
            messageText.Append(update.ChannelPost.Text);

        messageText.Append(Footer);
        
        files = files.Distinct().ToList();
    }
}