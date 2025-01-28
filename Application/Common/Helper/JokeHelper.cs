using System.Text.RegularExpressions;
using Application.Services;

namespace Application.Common.Helper;

public static class JokeHelper
{
    private static readonly string JokePlaceholder = "Внимание, анекдот!\n\n{0}";
    private static readonly string JokeBPlaceholder = "Внимание, анекдот категории Б!\n\n{0}";

    public delegate Task<string> JokeHandler(IHttpClientService httpClientService);

    public static async Task<string> GetJoke(IHttpClientService httpClientService)
    {
        string jokeText;
        do
        {
            jokeText = await GetJokeText(httpClientService);
        } while (string.IsNullOrWhiteSpace(jokeText));

        return string.Format(JokePlaceholder, jokeText);
    }

    public static async Task<string> GetBJoke(IHttpClientService httpClientService)
    {
        string jokeText;
        do
        {
            jokeText = await GetBJokeText(httpClientService);
        } while (string.IsNullOrWhiteSpace(jokeText));

        return string.Format(JokeBPlaceholder, jokeText);
    }

    public static async Task<string> GetRandomJoke(IHttpClientService httpClientService)
    {
        var random = new Random();
        var rnd = random.Next(0, 2);
        return rnd switch
        {
            0 => await GetJoke(httpClientService),
            1 => await GetBJoke(httpClientService),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Task<string> GetSnailJoke()
    {
        var message = @"Улитка заходит в бар, а бармен говорит: ""Мы не обслуживаем улиток!"" — и ногой выпихивает ее на улицу. Через неделю улитка возвращается в бар и говорит бармену: ""Ну и нахуя ты это сделал!?""";

        return Task.FromResult(message);
    }

    private static async Task<string> GetJokeText(IHttpClientService httpClientService)
    {
        var pageContent = await httpClientService.GetStringAsync("https://anekdot.ru/random/anekdot/"); //await httpClient.GetAsync("https://anekdot.ru/random/anekdot/").Result.Content.ReadAsStringAsync();

        var regex = new Regex(@"<div class=""topicbox"".*?<div class=""text"">(.*?)<\/div>");
        var rawJoke = regex.Match(pageContent).Groups[1].Value;

        regex = new Regex(@"<\s*?br\s*?\/?\s*?>");

        var result = regex
            .Replace(rawJoke, "\n")
            .Trim()
            .Replace("\n\n", "\n");

        return result;
    }

    private static async Task<string> GetBJokeText(IHttpClientService httpClientService)
    {
        var pageContent = await httpClientService.GetStringAsync("https://baneks.site/random"); //await httpClient.GetAsync("https://baneks.site/random").Result.Content.ReadAsStringAsync();

        var regex = new Regex(@"<section itemprop=""description"">([\s\S]*?)<\/section>");
        var rawJoke = regex.Match(pageContent).Groups[1].Value;

        regex = new Regex(@"<\s*?br\s*?\/?\s*?>");

        var result = regex
            .Replace(rawJoke, "\n")
            .Trim()
            .Replace("\n\n", "\n");
        
        regex = new Regex(@"<p>(.*?)<\/p>");
        
        var matches = regex.Matches(result).Select(x => x.Groups[1].Value).ToList();
        return string.Join('\n', matches);
    }
}