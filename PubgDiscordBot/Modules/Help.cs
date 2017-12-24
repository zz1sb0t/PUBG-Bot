using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace PubgDiscordBot.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task helpCommand()
        {
            await Context.Channel.SendMessageAsync
                ("```Commands:\n" +
                "---------------------------------------------------\n" +
                "pubglastgame \n" +
                "watch \n" +
                "removewatch \n" +
                "watchlist \n" +
                "---------------------------------------------------\n" +
                "Type $help-<command> to get more information```");
        }
        [Command("help-pubglastgame")]
        public async Task helpPubglastgameCommand()
        {
            await Context.Channel.SendMessageAsync(HelpMessages.HelpPubgLastGame.Message);
        }
        [Command("help-watch")]
        public async Task helpWatchCommand()
        {
            await Context.Channel.SendMessageAsync(HelpMessages.HelpWatch.Message);
        }
        [Command("help-removewatch")]
        public async Task helpRemoveWatchCommand()
        {
            await Context.Channel.SendMessageAsync(HelpMessages.HelpRemoveWatch.Message);
        }
        [Command("help-watchlist")]
        public async Task helpWatchListCommand()
        {
            await Context.Channel.SendMessageAsync(HelpMessages.HelpWatchList.Message);
        }
        
    }
}
