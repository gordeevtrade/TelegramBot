using ExchangeService.BankProvider.Models;

namespace TelegramBot.BankProvider.Interface
{
    public interface IBankProvider
    {
        Task<JsonExchangeRate> GetExchangeRate(string currencyCode, string formattedDatedate);
    }
}