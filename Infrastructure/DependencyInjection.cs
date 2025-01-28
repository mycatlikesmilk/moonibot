using Application.Services;
using Application.Services.ExternalServices;
using Infrastructure.Services;
using Infrastructure.Services.External;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // main discord service
        // services.AddSingleton<IDiscordService, DiscordService>();
        // services.AddHostedService<DiscordService>(provider => (provider.GetRequiredService<IDiscordService>() as DiscordService)!);
        
        services.AddHttpClient<IHttpClientService, HttpClientService>();
        services.AddSingleton<ICyberboomationService, CyberboomationService>();
        services.AddSingleton<ITwitchService, TwitchService>();
        services.AddHostedService<CyberboomationService>(provider => (provider.GetRequiredService<ICyberboomationService>() as CyberboomationService)!);
        services.AddHostedService<TwitchService>(provider => (provider.GetRequiredService<ITwitchService>() as TwitchService)!);
        return services;
    }
}