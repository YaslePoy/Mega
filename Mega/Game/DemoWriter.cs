using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    internal static class DemoWriter
    {
        private static int currentLine;
        static List<string> demoStrings;
        static StringBuilder line;
        public static bool Write, Read, Nexting;
        static DemoWriter()
        {
            line = new StringBuilder();
            demoStrings = new List<string>();
        }
        public static void TakePhoto(World world)
        {
            if (!Write)
                return;
            addFloat(world.Player.Cam.Pitch);
            addFloat(world.Player.Cam.Yaw);
            addFloat(world.Player.Moving.X);
            addFloat(world.Player.Moving.Y);

            addLine();
        }

        static void addLine()
        {
            demoStrings.Add(line.ToString());
            line.Clear();
        }

        public static void SaveDemo(string path = "demo.dd")
        {
            File.WriteAllLines(path, demoStrings);
        }

        public static void LoadDemo(string path = "demo.dd")
        {
            demoStrings = File.ReadAllLines(path).ToList();
        }

        public static void ApplyPhoto(World world)
        {

            if (currentLine >= demoStrings.Count || !Read)
                return;
            var lineParts = demoStrings[currentLine].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var floats = lineParts.Select(float.Parse).ToArray();
            world.Player.Cam.Pitch = floats[0];
            world.Player.Cam.Yaw = floats[1];
            world.Player.Moving.X = floats[2];
            world.Player.Moving.Y = floats[3];

        }

        public static void NextFrame()
        {
            if (Read)
                currentLine++;
        }
        static void addFloat(float f)
        {
            line.Append(f);
            line.Append(' ');
        }
    }
}
