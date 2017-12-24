using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgDiscordBot.Modules
{
    public class GetLastPubgGame : ModuleBase<SocketCommandContext>
    {
        [Command("PubgLastGame")]
        public async Task ReplayCommand(string name = null, string server = null)
        {
            try
            {
                if(name == null || server == null)
                {
                    await Context.Channel.SendMessageAsync(
                        "```------------------------------------------------\n" +
                        "The format is $pubglastgame {username} {server}\n" +
                        "Get more information by typing $help-pubglastgame```");
                    return;
                }
                NameValueCollection values = new NameValueCollection();
                values = PubgInfo.PubgInfo.GetLastGame(name, server);

                string GameType = values.Get("GameType");
                string Kills = values.Get("Kills");
                string DamageDone = values.Get("DamageDone");
                string RankingChange = values.Get("RankingChange");
                string Place = values.Get("Place");
                await Context.Channel.SendMessageAsync
                    ($"```Last game statistics for {name} in {server} server \n" +
                    $"--------------------------------------------------------\n" +

                    $"Game Type: {GameType} \n" +
                    $"Kills: {Kills} \n" +
                    $"Damage Done: {DamageDone} \n" +
                    $"Place: {Place} \n" +
                    $"Ranking Change: {RankingChange}```");
            }
            catch
            {
                await Context.Channel.SendMessageAsync(
                    "```-------------------------------------------------\n" +
                    "Can't find the specified user \n" +
                    "Get more information by typing $help-pubglastgame```");
            }
            
        }
        [Command("Watch")]
        public async Task AddToWatchList(string username = null, string server = null, string discordChannel = null)
        {
            if (username == null || server == null || discordChannel == null)
            {
                await Context.Channel.SendMessageAsync(
                    "```--------------------------------------------------------------\n" +
                    "The format is $watch {username} {server} {discord channel name} \n" +
                    "Get more information by typing $help-watch```");
                return;
            }
            var TextChannel = GetTextChannelByName(discordChannel);
            if (TextChannel == null) // Checking if text channel exists
            {
                await Context.Channel.SendMessageAsync("Can't find text channel named " + discordChannel);
            }
            else if (!PubgInfo.PubgCheckForNewGames.DoesUserExistInDB(username, server)) // Checking if user exists in DB
            {
                await Context.Channel.SendMessageAsync($"Can't find find player called {username} in {server.ToUpper()} server");
            }
            else
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pubg", TextChannel.Id + ".xml");
                if (!File.Exists(path))
                {
                    PubgInfo.PubgCheckForNewGames.CreateXML(path);
                }
                await Context.Channel.SendMessageAsync(PubgInfo.PubgCheckForNewGames.AddName(path, username, server) + " in " + TextChannel.Name);
            }
        }
        [Command("RemoveWatch")]
        public async Task RemoveFromWatchList(string username = null, string server = null, string discordChannel = null)
        {
            if(username == null || server == null || discordChannel == null)
            {
                await Context.Channel.SendMessageAsync(
                    "```--------------------------------------------------------------------\n" +
                    "The format is $removewatch {username} {server} {discord channel name} \n" +
                    "Get more information by typing $help-removewatch```");
                return;
            }
            var TextChannel = GetTextChannelByName(discordChannel);
            if (TextChannel == null) // Checking if text channel exists
            {
                await Context.Channel.SendMessageAsync("Can't find text channel named " + discordChannel);
            }
            else
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pubg", TextChannel.Id + ".xml");
                if (!File.Exists(path))
                {
                    await Context.Channel.SendMessageAsync("User can not be found in the watch list");
                }
                else
                {
                    await Context.Channel.SendMessageAsync(PubgInfo.PubgCheckForNewGames.RemoveName(path, username, server) + " in " + TextChannel.Name);
                }
            }
        }
        [Command("WatchList")]
        public async Task WatchList(string channelName = null)
        {
            string path;
            List<string> names = new List<string>();
            SocketTextChannel textChannel;
            
            if(channelName == null)
            {
                textChannel = GetTextChannelByName(Context.Channel.Name);
                names = PubgInfo.PubgCheckForNewGames.GetWatchList(Context.Channel.Id.ToString());
                if(names == null)
                {
                    await Context.Channel.SendMessageAsync($"The channel: {textChannel.Name} is currently not watching anyone!");
                    return;
                }
            }
            else
            {
                textChannel = GetTextChannelByName(channelName);
                if(textChannel == null)
                {
                    await Context.Channel.SendMessageAsync("Can't find the specified text channel");
                    return;
                }
                else
                {
                    names = PubgInfo.PubgCheckForNewGames.GetWatchList(textChannel.Id.ToString());
                    if(names == null)
                    {
                        await Context.Channel.SendMessageAsync($"The channel: {textChannel.Name} is currently not watching anyone!");
                        return;
                    }
                }
            }
            if(names.Count == 0)
            {
                await Context.Channel.SendMessageAsync($"The channel: {textChannel.Name} is currently not watching anyone!");
                return;
            }
            string message = "```Currently watched players are: ";
            foreach(var name in names)
            {
                message += $"\n{name}";
            }
            message += "```";
            await Context.Channel.SendMessageAsync(message);
        }
        public SocketTextChannel GetTextChannelByName(string name)
        {
            foreach (var TextChannel in Context.Guild.TextChannels)
            {
                if (TextChannel.Name == name)
                {
                    return TextChannel;
                }
            }
            return null;
        }

    }
}
