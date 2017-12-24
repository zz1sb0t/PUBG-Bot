using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgDiscordBot.HelpMessages
{
    public static class HelpRemoveWatch
    {
        public static string Message { get; } = "```REMOVEWATCH \n" +
                "----------------------------------------------------\n" +
                "INFORMATION: \n" +
                "Removes the watched player from the text channel specified..\n" +
                "----------------------------------------------------\n" +
                "HOW TO USE:\n" +
                "$removewatch {username} {server} {discord text channel name} \n" +
                "EXAMPLE: $removewatch postimees eu pubg-games\n" +
                "EXAMPLE2: $removewatch shroud na general\n" +
                "----------------------------------------------------\n" +
                "SERVERS:\n" +
                "NA, AS, KRJP, KAKAO, SA, EU, OC, SEA```";
    }
}
