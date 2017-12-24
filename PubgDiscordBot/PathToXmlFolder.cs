using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgDiscordBot
{
    public static class PathToXmlFolder
    {
        public static string GetPath()
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pubg");
        }
    }
}
