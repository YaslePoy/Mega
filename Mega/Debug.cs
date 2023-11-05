using System.Text;

namespace Mega
{
    internal static class Debug
    {
        public static bool SaveEnable;
        static StringBuilder logs = new StringBuilder();
        public static void Log(string message, int level = 0)
        {
            var msg = string.Concat(string.Join("", Enumerable.Repeat("    ", level)), message);
            Console.WriteLine(msg);
        }
        public static void LogToFile(string message, int level = 0)
        {
            if (!SaveEnable)
                return;

            logs.Append(string.Join("", Enumerable.Repeat("    ", level)));
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

        internal static void Trap()
        {
        }
    }
}
