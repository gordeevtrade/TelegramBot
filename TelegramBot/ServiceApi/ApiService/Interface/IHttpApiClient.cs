namespace TelegramBot.ApiService.Interface
{
    public interface IHttpApiService
    {
        Task<string> SendHttpRequestAsync(string url);
    }
}