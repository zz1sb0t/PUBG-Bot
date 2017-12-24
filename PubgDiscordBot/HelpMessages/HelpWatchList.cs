using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgDiscordBot.HelpMessages
{
    public static class HelpWatchList
    {
        public static string Message { get; } = ("```WATCHLIST \n" +
                "----------------------------------------------------\n" +
                "INFORMATION: \n" +
                "Shows the users that are watched by the channel\n" +
                "----------------------------------------------------\n" +
                "HOW TO USE: \n" +
                "$watchlist \n" +
                "Looks this channel watch list\n" +
                "OR\n" +
                "$watchlist {text channel name\n}" +
                "Looks at the specified channel watch list\n" +
                "EXAMPLE: $watchlist \n" +
                "EXAMPLE2: $watchlist general```");
    }
}
