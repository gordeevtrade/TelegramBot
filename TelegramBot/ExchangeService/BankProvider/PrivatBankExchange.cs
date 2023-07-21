using ExchangeService.BankProvider.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using TelegramBot.ApiService.Interface;
using TelegramBot.BankProvider.Interface;

namespace TelegramBot.BankProvider
{
    public class PrivatBankExchange : IBankProvider

    {
        private IHttpApiService _httpClient;
        private string _privatBankApi;

        public PrivatBankExchange(IHttpApiService httpApiClient, string apiUrl)
        {
            _httpClient = httpApiClient;
            _privatBankApi = apiUrl;
        }

        public async Task<JsonExchangeRate> GetExchangeRate(string currencyCode, string formattedDatedate)
        {
            if (!IsValidInput(currencyCode, formattedDatedate))
            {
                throw new ArgumentException("Invalid input");
            }
            else
            {
                try
                {
                    string url = $"{_privatBankApi}?json&date={formattedDatedate}";
                    string responseFromServer = await _httpClient.SendHttpRequestAsync(url);
                    var rate = ParseJson(responseFromServer, currencyCode, formattedDatedate);

                    if (rate == null)
                    {
                        throw new Exception("Failed to parse result");
                    }
                    return rate;
                }
                catch (Exception ex)
                {
                    throw new Exception("not_have_data");
                }
            }
        }

        private JsonExchangeRate ParseJson(string json, string currencyCode, string formattedDatedate)
        {
            var privatBankResponse = JsonConvert.DeserializeObject<PrivatBankResponse>(json);

            var rate = privatBankResponse.ExchangeRate.FirstOrDefault(r => r.Currency == currencyCode.ToUpper());

            return rate;
        }

        private bool IsValidInput(string currencyCode, string formattedDate)
        {
            if (currencyCode.Length != 3)
            {
                return false;
            }

            if (!Regex.IsMatch(currencyCode, @"^[A-Za-z]+$"))

            {
                return false;
            }

            if (!DateTime.TryParseExact(formattedDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return false;
            }
            return true;
        }
    }
}