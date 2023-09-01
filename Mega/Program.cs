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
                Size = new Vector2i(1600, 900),
                Title = "LearnOpenTK - Camera",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
            };

            using (var window = new Window(new GameWindowSettings() { UpdateFrequency = 50 }, nativeWindowSettings) { })
            {
                window.Run();
            }

        }
    }
}
