using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgDiscordBot.Modules
{
    public static class Log
    {
        public static void LogText(string message)
        {
            try
            {
                using (StreamWriter w = File.AppendText(@"D:\Projektid\DiscordBotTA16E\Log.txt"))
                {
                    w.WriteLine($"[{DateTime.Now}] {message}");
                    Console.WriteLine($"[{DateTime.Now}] {message}");
                    w.Close();
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now}] Error logging: {message}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        public static void ErrorLog(string message)
        {
            try
            {
                using (StreamWriter w = File.AppendText(@"D:\Projektid\DiscordBotTA16E\ErrorLog.txt"))
                {
                    w.WriteLine($"[{DateTime.Now}] {message}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now}] {message}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    w.Close();
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now}] Error logging: {message}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
