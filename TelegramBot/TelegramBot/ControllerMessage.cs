using ExchangeService.BankProvider.Models;
using ExchangeService.Interface;
using LocalizationLanguagePackage;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.Enum;

namespace TelegramBot
{
    public class ControllerMessage
    {
        private ITelegramBotClient _botClient;
        private IExchangeServiceProvider _exchangeRateService;
        private MenuBuilder _menuBuilder;
        private InputState currentInputState = InputState.None;
        private string _recentlyEnteredCurrency = "";

        public ControllerMessage(ITelegramBotClient botClient, IExchangeServiceProvider exchangeRateService, MenuBuilder menuBuilder)
        {
            _botClient = botClient;
            _exchangeRateService = exchangeRateService;
            _menuBuilder = menuBuilder;
        }

        public async Task HandleCallbackQuery(CallbackQueryEventArgs e)
        {
            var data = e.CallbackQuery.Data;

            if (data == "/go")
            {
                currentInputState = InputState.AwaitingCurrencyInput;
                LanguagePackage.userLanguages[e.CallbackQuery.Message.Chat.Id] = "en";

                await _botClient.SendTextMessageAsync(
                    chatId: e.CallbackQuery.Message.Chat.Id,
                    text: LanguagePackage.GetLocalizedMessage("currency_prompt", e.CallbackQuery.Message.Chat.Id)
                );
            }
            else if (data == "/language")
            {
                await СhoiceLanguage(e.CallbackQuery.Message.Chat.Id, data);
            }
            else
            {
                LanguagePackage.userLanguages[e.CallbackQuery.Message.Chat.Id] = data;
                await _botClient.SendTextMessageAsync(
                    chatId: e.CallbackQuery.Message.Chat.Id,
                    text: LanguagePackage.GetLocalizedMessage("currency_prompt", e.CallbackQuery.Message.Chat.Id)
                );
            }
        }

        public async Task InitialMenu(long chatId)
        {
            var keyboard = _menuBuilder.InitialMenu();

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Welcome! Please select an option:",
                replyMarkup: keyboard
            );
        }

        public async Task СhoiceLanguage(long chatId, string currencyName)
        {
            currentInputState = InputState.AwaitingLanguageInput;

            var keyboard = _menuBuilder.ChoiceLanguage();

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: " Choose language",
                replyMarkup: keyboard
            );

            currentInputState = InputState.AwaitingCurrencyInput;
        }

        public async Task ProcessUserInput(long chatId, string userInput)
        {
            switch (currentInputState)
            {
                case InputState.AwaitingCurrencyInput:

                    await AcceptingCurrencyFromUser(chatId, userInput);
                    break;

                case InputState.AwaitingDateInput:
                    await AcceptingDateFromUser(chatId, userInput);
                    break;

                default:
                    await _botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "\r\n /start");
                    break;
            }
        }

        public async Task DeactivateBot(long chatId)
        {
            await _botClient.SendTextMessageAsync(
    chatId: chatId,
    text: LanguagePackage.GetLocalizedMessage("stop_bot", chatId)
     );
            _botClient.StopReceiving();

            _botClient.StartReceiving();
            currentInputState = InputState.None;
        }

        public async Task AcceptingCurrencyFromUser(long chatId, string currencyName)
        {
            currentInputState = InputState.AwaitingDateInput;
            _recentlyEnteredCurrency = currencyName;

            await _botClient.SendTextMessageAsync(
             chatId: chatId,
             text: LanguagePackage.GetLocalizedMessage("date_prompt", chatId)
         );
        }

        public async Task AcceptingDateFromUser(long chatId, string dateInput)
        {
            currentInputState = InputState.AwaitingCurrencyInput;
            await ReturnExchangeRate(chatId, dateInput);
        }

        public async Task ReturnExchangeRate(long chatId, string date)
        {
            try
            {
                JsonExchangeRate exchangeRateResult = await _exchangeRateService.GetExchangeRate(_recentlyEnteredCurrency, date);

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"{LanguagePackage.GetLocalizedMessage("exchange_course", chatId)}   {exchangeRateResult.Currency}    {exchangeRateResult.SaleRate}  /  {exchangeRateResult.PurchaseRate}"
                );
            }
            catch (Exception ex)
            {
                var errorMessages = new Dictionary<string, string>
    {
        { "Invalid input", "incorrect_data" },
        { "Failed to parse result", "course_notFound" },
        { "not_have_data", "not_have_data" }
    };

                if (errorMessages.ContainsKey(ex.Message))
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: LanguagePackage.GetLocalizedMessage(errorMessages[ex.Message], chatId)
                    );
                }
                else
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: ex.Message
                    );
                }
            }

            await Task.Delay(1300);

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: LanguagePackage.GetLocalizedMessage("next_currency_or_stop", chatId)
            );
        }
    }
}