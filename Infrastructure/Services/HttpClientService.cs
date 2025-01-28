using Application.Services;

namespace Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> GetStringAsync(string url)
    {
        return await _httpClient.GetStringAsync(url);
    }
}