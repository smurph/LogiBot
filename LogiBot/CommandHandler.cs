using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace LogiBot
{
    internal class CommandHandler
    {
        private List<char> _commandTriggers;
        private char _commandTrigger = '?';
        private char _altCommandTrigger = '!';

        private DiscordSocketClient _client;
        private CommandService _service;

        public CommandHandler()
        {
            InitCommandTriggers();
            _client = LogiBot.GetInstance().GetClient();
            Init();
        }

        public CommandHandler(DiscordSocketClient client)
        {
            InitCommandTriggers();
            _client = client;
            Init();
        }

        private void InitCommandTriggers()
        {
            _commandTriggers = new List<char>()
            {
                '!',
                '?'
            };
        }

        private void Init()
        {
            _service = new CommandService();

            _service.AddModulesAsync(Assembly.GetEntryAssembly());

            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;

            Console.WriteLine(msg.Author.ToString() + "> " + msg.Content);

            var context = new SocketCommandContext(_client, msg);

            if (HasCommandPrefix(msg)) //.HasCharPrefix(_commandTrigger, ref argPosition) || msg.HasCharPrefix(_altCommandTrigger, ref argPosition))
            {
                var result = await _service.ExecuteAsync(context, 1);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }

        private bool HasCommandPrefix(SocketUserMessage msg)
        {
            int argPosition = 0;
            foreach (var cmdTrigger in _commandTriggers)
            {
                if (msg.HasCharPrefix(cmdTrigger, ref argPosition)) return true;
            }

            return false;
        }
    }
}