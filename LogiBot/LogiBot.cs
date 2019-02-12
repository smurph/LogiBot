using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Configuration;

namespace LogiBot
{
    public class LogiBot
    {
        private static LogiBot _instance = null;

        public static LogiBot GetInstance()
        {
            if (_instance == null) _instance = new LogiBot();

            return _instance;
        }

        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public DiscordSocketClient GetClient()
        {
            return _client;
        }

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, ConfigurationSettings.AppSettings["UserToken"]);

            await _client.StartAsync();

            _handler = new CommandHandler(_client);

            await Task.Delay(-1);
        }
    }
}
