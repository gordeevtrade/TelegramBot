using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    public class MenuBuilder
    {
        public InlineKeyboardMarkup InitialMenu()
        {
            return new InlineKeyboardMarkup(new[]
            {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Go", "/go"),
                InlineKeyboardButton.WithCallbackData("Language", "/language"),
            }
        });
        }

        public InlineKeyboardMarkup ChoiceLanguage()
        {
            return new InlineKeyboardMarkup(new[]
            {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Русский", "ru"),
                InlineKeyboardButton.WithCallbackData("English", "en"),
                InlineKeyboardButton.WithCallbackData("Украінська ", "ua")
            }
        });
        }
    }
}