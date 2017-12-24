using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace PubgInfo
{
    public static class PubgCheckForNewGames
    {

        //static string path = AppDomain.CurrentDomain.BaseDirectory + "/Pubg.xml";
        public static void CheckUser()
        {
            //CheckAllGames();

        }
        public static List<PubgDiscordBot.PubgGameData> CheckAllGamesInAllServers()
        {
            List<string> messages = new List<string>();
            List<PubgDiscordBot.PubgGameData> Datas = new List<PubgDiscordBot.PubgGameData>();

            var files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pubg"));
            foreach (var file in files)
            {
                messages = CheckAllGames(file);
                if (messages.Count > 0)
                {
                    PubgDiscordBot.PubgGameData data = new PubgDiscordBot.PubgGameData();
                    data.ChannelID = Convert.ToUInt64(Path.GetFileNameWithoutExtension(file));
                    data.Messages = messages;
                    Datas.Add(data);
                }
            }
            return Datas;
        }

        public static List<string> CheckAllGames(string path)
        {
            List<string> strings = new List<string>();
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            var nodes = xml.SelectNodes($"/MATCHES/MATCH");
            foreach (XmlNode node in nodes)
            {
                string name = node.Attributes["username"].Value;
                string server = node.Attributes["server"].Value;
                string text = CheckLastGame(name, server, path);
                if (text == null)
                    continue;
                else
                {
                    strings.Add(text);
                }
            }
            return strings;
        }
        public static string RemoveName(string pathToXml, string username, string server)
        {
            if (DoesUserExist(username, server, pathToXml))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(pathToXml);
                RemoveNameFromXML(xml, username, server, pathToXml);

                return $"{username} | {server.ToUpper()} Removed from the watch list";
            }
            else
            {
                return $"{username} | {server.ToUpper()} can't be found in the watch list";
            }

        }
        public static string AddName(string pathToXml, string username, string server)
        {
            if (!DoesUserExist(username, server, pathToXml))
            {
                AddNameToXML(username, server, pathToXml);
                PubgDiscordBot.Modules.Log.LogText($"Adding {username} in {server.ToUpper()} to {pathToXml}");
                return $"{username} | {server.ToUpper()} added to watch list";
            }
            return $"{username} | {server.ToUpper()} is already in the watch list";

        }

        static string CheckLastGame(string username, string server, string path)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            NameValueCollection lastGameValues = PubgInfo.GetLastGame(username, server);

            string LastCheckedKills = GetPrevKills(xml, username, server);
            string LastCheckedDamage = GetPrevDamage(xml, username, server);
            string LastCheckedDistance = GetPrevDistance(xml, username, server);

            string thisGameKills = lastGameValues.Get("Kills");
            string thisGameDamage = lastGameValues.Get("DamageDone");
            string thisGameDistance = lastGameValues.Get("Distance").Replace(" ", "").Replace("\n", " ");

            if (thisGameKills == LastCheckedKills && thisGameDamage == LastCheckedDamage && thisGameDistance == LastCheckedDistance)
            {
                GameAlreadyChecked(username, server);
                return null;
            }
            else
            {
                NewGameFound(xml, thisGameKills, thisGameDamage, thisGameDistance, username, server, path);

                string GameType = lastGameValues.Get("GameType");
                string Kills = lastGameValues.Get("Kills");
                string DamageDone = lastGameValues.Get("DamageDone");
                string RankingChange = lastGameValues.Get("RankingChange");
                string Place = lastGameValues.Get("Place");
                string text =
                    ($"```New game completed for {username} in {server} server \n" +
                    $"--------------------------------------------------------\n" +

                    $"Game Type: {GameType} \n" +
                    $"Kills: {Kills} \n" +
                    $"Damage Done: {DamageDone} \n" +
                    $"Place: {Place} \n" +
                    $"Ranking Change: {RankingChange}```");
                return text;
            }
        }
        static void GameAlreadyChecked(string username, string server)
        {

        }
        static void NewGameFound(XmlDocument xml, string kills, string damage, string distance, string username, string server, string path)
        {
            SetPrevKills(xml, username, server, kills, path);
            SetPrevDamage(xml, username, server, damage, path);
            SetPrevDistance(xml, username, server, distance, path);
        }
        static string GetPrevKills(XmlDocument xml, string username, string server)
        {
            XmlNode node = xml.SelectSingleNode($"/MATCHES/MATCH[@username='{username}'][@server='{server}']/kills");
            return node.InnerText;
        }
        static string GetPrevDistance(XmlDocument xml, string username, string server)
        {
            XmlNode node = xml.SelectSingleNode($"/MATCHES/MATCH[@username='{username}'][@server='{server}']/distance");
            return node.InnerText;
        }
        static string GetPrevDamage(XmlDocument xml, string username, string server)
        {
            XmlNode node = xml.SelectSingleNode($"/MATCHES/MATCH[@username='{username}'][@server='{server}']/damage");
            return node.InnerText;
        }
        static void SetPrevKills(XmlDocument xml, string username, string server, string newValue, string XMLpath)
        {
            XmlNode node = xml.SelectSingleNode($"/MATCHES/MATCH[@username='{username}'][@server='{server}']/kills");
            node.InnerText = newValue;
            xml.Save(XMLpath);
        }
        static void SetPrevDamage(XmlDocument xml, string username, string server, string newValue, string XMLpath)
        {
            XmlNode node = xml.SelectSingleNode($"/MATCHES/MATCH[@username='{username}'][@server='{server}']/damage");
            node.InnerText = newValue;
            xml.Save(XMLpath);
        }
        static void SetPrevDistance(XmlDocument xml, string username, string server, string newValue, string XMLpath)
        {
            XmlNode node = xml.SelectSingleNode($"/MATCHES/MATCH[@username='{username}'][@server='{server}']/distance");
            node.InnerText = newValue;
            xml.Save(XMLpath);
        }
        private static void RemoveNameFromXML(XmlDocument xml, string username, string server, string pathToSave)
        {
            XmlNode node = xml.SelectSingleNode($"/MATCHES/MATCH[@username='{username}'][@server='{server}']");
            if (node != null)
            {
                XmlNode parent = node.ParentNode;
                parent.RemoveChild(node);
                xml.Save(pathToSave);
            }
        }
        private static void AddNameToXML(string username, string server, string XMLpath)
        {
            XDocument doc = XDocument.Load(XMLpath);
            XElement root = new XElement("MATCH");
            root.Add(new XAttribute("username", username));
            root.Add(new XAttribute("server", server));
            root.Add(new XElement("kills", "0"));
            root.Add(new XElement("damage", "0"));
            root.Add(new XElement("distance", "0"));
            doc.Element("MATCHES").Add(root);
            doc.Save(XMLpath);
        }
        public static bool DoesUserExist(string username, string server, string XMLpath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(XMLpath);
            XmlNode node;
            XmlElement root = doc.DocumentElement;
            node = root.SelectSingleNode($"/MATCHES/MATCH[@username='{username}'][@server='{server}']");
            if (node != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void CreateXML(string XMLpath)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("MATCHES");
            doc.Add(root);
            doc.Save(XMLpath);
        }
        public static bool DoesUserExistInDB(string username, string server)
        {
            try
            {
                PubgInfo.GetLastGame(username, server);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static List<string> GetWatchList(string channelID)
        {
            string path = Path.Combine(PubgDiscordBot.PathToXmlFolder.GetPath(), channelID + ".xml");
            if(!File.Exists(path))
            {
                return null;
            }
            List<string> strings = new List<string>();
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            var nodes = xml.SelectNodes($"/MATCHES/MATCH");
            foreach (XmlNode node in nodes)
            {
                string name = node.Attributes["username"].Value;
                string server = node.Attributes["server"].Value;
                string combined = $"{name} | {server.ToUpper()}";
                strings.Add(combined);
            }
            return strings;
        }
    }

}
