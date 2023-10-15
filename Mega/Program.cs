using Mega.Game;
using Mega.Video;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Mega
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1280, 720),
                Title = "Mega Game",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,

            };
            using (var window = new Window(new GameWindowSettings() /*{ UpdateFrequency = 100 }*/, nativeWindowSettings) { })
            {
                //Task.Run(() => { Thread.Sleep(10000); window.Close(); });

                window.Run();
            }
        }
    }
}
