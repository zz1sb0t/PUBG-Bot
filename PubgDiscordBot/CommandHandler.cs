using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PubgDiscordBot
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;

        private CommandService _service;


        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;

            _service = new CommandService();

            _service.AddModulesAsync(Assembly.GetEntryAssembly());

            _client.MessageReceived += _client_MessageReceived;

            CheckPubgPlays(client);


        }
        public async Task CheckPubgPlays(DiscordSocketClient client)
        {
            while (true)
            {
                try
                {
                    var Games = PubgInfo.PubgCheckForNewGames.CheckAllGamesInAllServers();
                    foreach (var game in Games)
                    {
                        foreach (var message in game.Messages)
                        {
                            await ((ISocketMessageChannel)client.GetChannel(game.ChannelID)).SendMessageAsync(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    PubgDiscordBot.Modules.Log.ErrorLog("Failed to check games Error: " + ex.Message);
                }

                await Task.Delay(60000);
            }
        }


        private async Task _client_MessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;

            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;
            if (msg.HasCharPrefix('$', ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
