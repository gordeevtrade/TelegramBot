namespace LocalizationLanguagePackage
{
    public static class LanguagePackage
    {
        public static Dictionary<long, string> userLanguages = new Dictionary<long, string>();

        public static Dictionary<string, Dictionary<string, string>> messages = new Dictionary<string, Dictionary<string, string>>
    {
        { "currency_prompt", new Dictionary<string, string> { { "en", "Specify the Currency (Example Usd/usd)" }, { "ru", "Укажите валюту (Пример Usd/usd)" }, { "ua", "Вкажіть валюту (Приклад Usd/usd)" } } },
        { "date_prompt", new Dictionary<string, string> { { "en", "Enter the date (Example 10.10.2020)" }, { "ru", "Введите дату (Пример 10.10.2020)" }, { "ua", "Введіть дату (Приклад 10.10.2020)" } } },
        { "next_currency_or_stop", new Dictionary<string, string> { { "en", "To continue, please provide the New Currency or type /stop to stop the Bot\r\n\r\n" }, { "ru", "Для продолжения укажите Новую Валюту или напишите /stop для остановки Бота\r\n\r\n" }, { "ua", "Для продовження вкажіть Нову Валюту або напишіть /stop, щоб зупинити Бота\r\n\r\n\r\n\r\n\r\n" } } },
        { "stop_bot", new Dictionary<string, string> { { "en", "Bot stopped! To activate the bot, enter /start!" }, { "ru", "Бот остановлен! Для активации бота введите /start!" }, { "ua", "Бот зупинено! Для активації бота введіть /start!" } } },
         { "incorrect_data", new Dictionary<string, string> { { "en", " Incorrect data entered. Please check the currency code and date format.\r\n\r\n" }, { "ru", "Введены некорректные данные. Пожалуйста, проверьте код валюты и формат даты." }, { "ua", "Введено некоректні дані. Будь ласка, перевірте код валюти та формат дати." } } },
         { "course_notFound", new Dictionary<string, string> { { "en", "Exchange rate not found. Please try again later or provide different data..\r\n\r\n" }, { "ru", "Курс не найден. Попробуйте позже или укажите другие данные." }, { "ua", " Курс не знайдений. Будь ласка, спробуйте пізніше або вкажіть інші дані." } } },
         { "not_have_data", new Dictionary<string, string> { { "en", "No data found for your request. Please try again\r\n\r\n" }, { "ru", " По вашему запросу нет данных. Попробуйте еще раз" }, { "ua", " За вашим запитом немає даних. Спробуйте ще раз" } } },

    { "exchange_course", new Dictionary<string, string> { { "en", "Exchange course оn" }, { "ru", "Обменный Курс по " }, { "ua", "Обмінний Курса по :" } } },
        };

        public static string GetLocalizedMessage(string messageId, long chatId)
        {
            if (userLanguages.TryGetValue(chatId, out string language))
            {
                if (messages.TryGetValue(messageId, out Dictionary<string, string> messageVariants))
                {
                    if (messageVariants.TryGetValue(language, out string message))
                    {
                        return message;
                    }
                }
            }

            return messages[messageId]["en"];
        }
    }
}