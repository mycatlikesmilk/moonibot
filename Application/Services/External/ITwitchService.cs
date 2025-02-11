using TwitchLib.Client;

namespace Application.Services.ExternalServices;

public interface ITwitchService
{
    TwitchClient Client { get; set; }
    string ConnectedChannel { get; set; }
}