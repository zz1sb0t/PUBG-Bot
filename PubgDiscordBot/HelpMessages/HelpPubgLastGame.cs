using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgDiscordBot.HelpMessages
{
    public static class HelpPubgLastGame
    {
        public static string Message { get; } = ("```PUBGLASTGAME \n" +
                "----------------------------------------------------\n" +
                "INFORMATION: \n" +
                "Shows some information of the last played game.\n" +
                "Note that information is saved after the winner has been announced. \n" +
                "----------------------------------------------------\n" +
                "HOW TO USE:\n" +
                "$pubglastgame {username} {server} \n" +
                "EXAMPLE: $pubglastgame postimees eu \n" +
                "EXAMPLE2: $pubglastgame shroud na \n" +
                "----------------------------------------------------\n" +
                "SERVERS:\n" +
                "NA, AS, KRJP, KAKAO, SA, EU, OC, SEA```");
    }
}
