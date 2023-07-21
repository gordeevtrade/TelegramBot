namespace TelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BotStarter botStarter = new BotStarter("config.json");
            botStarter.StartBot();
        }
    }
}