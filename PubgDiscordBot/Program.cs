using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace PubgDiscordBot
{
    public class Program
    {
        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, "Mzk0MTQ2NDA1MzM1MTcxMDgy.DSAFQw.qQAGdjYz6uWGu7FDxSWjpZUnows");
            await _client.StartAsync();

            _handler = new CommandHandler(_client);
            await Task.Delay(-1);
        }
    }
}
