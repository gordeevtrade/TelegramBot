using ExchangeService.BankProvider.Models;

namespace ExchangeService.Interface
{
    public interface IExchangeServiceProvider
    {
        Task<JsonExchangeRate> GetExchangeRate(string currencyCode, string formattedDatedate);
    }
}