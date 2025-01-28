using Telegram.Bot.Types;

namespace Application.Services.ExternalServices;

public interface ITelegramService
{
    Task<Update[]> GetUpdates(int offset);
}