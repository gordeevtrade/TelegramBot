using ExchangeService.BankProvider.Models;
using ExchangeService.Interface;
using TelegramBot.BankProvider.Interface;

public class ExchangeServiceProvider : IExchangeServiceProvider
{
    private IBankProvider _exchangeRateProvider;

    public ExchangeServiceProvider(IBankProvider bankProvider)
    {
        _exchangeRateProvider = bankProvider;
    }

    public async Task<JsonExchangeRate> GetExchangeRate(string currencyCode, string formattedDatedate)
    {
        JsonExchangeRate exchangeRateResult = await _exchangeRateProvider.GetExchangeRate(currencyCode, formattedDatedate);

        return exchangeRateResult;
    }
}