using ExchangeService.Interface;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot;

public class TelegramBots
{
    private ITelegramBotClient _botClient;
    private IExchangeServiceProvider _exchangeRateService;
    private MenuBuilder _menuBuilder;
    private ControllerMessage _messageHandler;

    public TelegramBots(string token, IExchangeServiceProvider exchangeRateService)
    {
        _botClient = new TelegramBotClient(token);
        _exchangeRateService = exchangeRateService;
        _menuBuilder = new MenuBuilder();
        _messageHandler = new ControllerMessage(_botClient, _exchangeRateService, _menuBuilder);
    }

    public void StartBot()
    {
        _botClient.OnMessage += Bot_OnMessage;
        _botClient.StartReceiving();
        _botClient.OnCallbackQuery += Bot_OnCallbackQuery;

        Console.WriteLine("Bot is started");
        Console.ReadLine();
        _botClient.StopReceiving();
    }

    private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
    {
        await _messageHandler.HandleCallbackQuery(e);
    }

    private async void Bot_OnMessage(object sender, MessageEventArgs e)
    {
        if (e.Message.Text != null)
        {
            await RespondToUserMessage(e.Message.Chat.Id, e.Message.Text);
        }
    }

    private async Task RespondToUserMessage(long chatId, string userInput)
    {
        if (userInput.Equals("/start", StringComparison.OrdinalIgnoreCase))
        {
            await _messageHandler.InitialMenu(chatId);
        }
        else if (userInput.Equals("/stop", StringComparison.OrdinalIgnoreCase))
        {
            _messageHandler.DeactivateBot(chatId);
        }
        else if (userInput.Equals("/language", StringComparison.OrdinalIgnoreCase))
        {
            _messageHandler.СhoiceLanguage(chatId, userInput);
        }
        else
        {
            await _messageHandler.ProcessUserInput(chatId, userInput);
        }
    }
}