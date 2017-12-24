using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace PubgInfo
{
    public class PubgInfo
    {

        public static NameValueCollection GetLastGame(string name, string server)
        {
            string url = $"https://pubg.op.gg/user/{name}?server={server}";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url);
            var game = document.DocumentNode.SelectSingleNode("//li[@data-selector='total-played-game-item']");
            NameValueCollection values = new NameValueCollection();
            values.Add("GameType", GetSquadType(game));
            values.Add("Kills", GetKills(game));
            values.Add("DamageDone", GetDamage(game));
            values.Add("RankingChange", GetRankingChange(game));
            values.Add("Place", GetLastAndFinalPlace(game));
            values.Add("Time", GetTime(game));
            values.Add("Distance", GetDistance(game));
            return values;

        }
        public static string GetSquadType(HtmlNode game)
        {
            var gameType = game.SelectSingleNode(@".//div[@class='played-game__mode']").InnerHtml;
            if (gameType == null) return "N/A";
            var answer = gameType;
            answer = answer.Split('\n')[2];
            answer = answer.Trim();
            return answer;
        }
        public static string GetKills(HtmlNode game)
        {
            var gameInfo = game.SelectSingleNode(".//div[@class='played-game__column played-game__column--kill']");
            gameInfo = gameInfo.SelectSingleNode(".//div[@class='played-game__value']");
            return gameInfo.InnerText;
        }
        public static string GetDamage(HtmlNode game)
        {
            var gameInfo = game.SelectSingleNode(".//div[@class='played-game__column played-game__column--damage']");
            gameInfo = gameInfo.SelectSingleNode(".//div[@class='played-game__value']");
            return gameInfo.InnerText;
        }
        public static string GetRankingChange(HtmlNode game)
        {
            var gameInfo = game.SelectSingleNode(".//div[@class='played-game__layout played-game__layout--game-list']");
            var gameInfo2 = gameInfo.SelectSingleNode(".//div[@class='played-game__value played-game__value--up']");
            try
            {
                var answer = gameInfo2.InnerText.Split(' ')[0];
                return answer + " UP";
            }
            catch
            {
                try
                {
                    gameInfo = gameInfo.SelectSingleNode(".//div[@class='played-game__value played-game__value--down']");
                    var answer = gameInfo.InnerText.Split(' ')[0];
                    return answer + " DOWN";
                }
                catch
                {
                    return "0";
                }

            }
        }
        public static string GetPlace(HtmlNode game)
        {
            var gameInfo = game.SelectSingleNode(".//div[@class='played-game__column played-game__column--rank']");
            gameInfo = gameInfo.SelectSingleNode(".//span[@class='played-game__my-ranking']");
            return gameInfo.InnerText;
        }
        public static string GetLastAndFinalPlace(HtmlNode game)
        {
            var gameInfo = game.SelectSingleNode(".//div[@class='played-game__column played-game__column--rank']");
            gameInfo = gameInfo.SelectSingleNode(".//div[@class='played-game__ranking']");
            return gameInfo.InnerText;
        }
        public static string GetTime(HtmlNode game)
        {
            var gameInfo = game.SelectSingleNode(".//div[@class='played-game__column played-game__column--status']");
            gameInfo = gameInfo.SelectSingleNode(".//div[@class='played-game__reload-time']");

            return gameInfo.InnerText;
        }
        public static string GetDistance(HtmlNode game)
        {
            var gameInfo = game.SelectSingleNode(".//div[@class='played-game__column played-game__column--distance']");
            gameInfo = gameInfo.SelectSingleNode(".//div[@class='played-game__value']");

            return gameInfo.InnerText;
        }

    }
}
