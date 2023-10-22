using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mega
{
    internal static class Debug
    {
        static StringBuilder logs = new StringBuilder();
        public static void Log(string message, int level = 0)
        {
            var msg = string.Concat(string.Join("", Enumerable.Repeat("    ", level)), message);
            Console.WriteLine(msg);
        }
        public static void LogToFile(string message)
        {
            logs.AppendLine(message);
        }
        public static void clearLog()
        {
            logs.Clear();
        }
        public static void SaveLogs()
        {
            File.WriteAllText("debug.log", logs.ToString());
        }
    }
}
