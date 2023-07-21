using TelegramBot.ApiService.Interface;

public class HttpApiService : IHttpApiService
{
    private HttpClient _httpClient = new HttpClient();

    public async Task<string> SendHttpRequestAsync(string url)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"-4");
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("-3", ex);
        }
    }
}