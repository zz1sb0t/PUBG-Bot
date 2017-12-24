using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgDiscordBot.HelpMessages
{
    public static class HelpWatch
    {
        public static string Message { get; } = ("```WATCH \n" +
                "----------------------------------------------------\n" +
                "INFORMATION: \n" +
                "Automatically displays a message of players last game after it's completed..\n" +
                "Note that information is displayed after the winner has been announced. \n" +
                "----------------------------------------------------\n" +
                "HOW TO USE:\n" +
                "$watch {username} {server} {discord text channel name} \n" +
                "EXAMPLE: $watch postimees eu pubg-games\n" +
                "EXAMPLE2: $watch shroud na general\n" +
                "----------------------------------------------------\n" +
                "SERVERS:\n" +
                "NA, AS, KRJP, KAKAO, SA, EU, OC, SEA```");
    }
}
