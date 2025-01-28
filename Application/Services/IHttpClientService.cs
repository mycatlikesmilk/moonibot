namespace Application.Services;

public interface IHttpClientService
{
    Task<string> GetStringAsync(string url);
}