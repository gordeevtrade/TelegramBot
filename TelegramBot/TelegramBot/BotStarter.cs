using ExchangeService.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TelegramBot.ApiService.Interface;
using TelegramBot.BankProvider;
using TelegramBot.BankProvider.Interface;

namespace TelegramBot
{
    public class BotStarter
    {
        public string Token { get; private set; }
        public string ApiUrl { get; private set; }

        public BotStarter(string configFilePath)
        {
            string configContent = File.ReadAllText(configFilePath);
            JsonDocument configJson = JsonDocument.Parse(configContent);
            Token = configJson.RootElement.GetProperty("BotSettings").GetProperty("Token").GetString();
            ApiUrl = configJson.RootElement.GetProperty("ApiSettings").GetProperty("PrivatBankApiUrl").GetString();
        }

        public void StartBot()
        {
            var services = new ServiceCollection();

            services.AddTransient<IHttpApiService, HttpApiService>();

            services.AddTransient<IBankProvider, PrivatBankExchange>(serviceProvider =>
            new PrivatBankExchange(serviceProvider.GetRequiredService<IHttpApiService>(), ApiUrl));

            services.AddTransient<IExchangeServiceProvider, ExchangeServiceProvider>(serviceProvider =>
           new ExchangeServiceProvider(serviceProvider.GetRequiredService<IBankProvider>()));

            services.AddTransient<TelegramBots>(serviceProvider =>
                new TelegramBots(Token, serviceProvider.GetRequiredService<IExchangeServiceProvider>()));

            var serviceProvider = services.BuildServiceProvider();

            var bots = serviceProvider.GetRequiredService<TelegramBots>();

            bots.StartBot();
        }
    }
}