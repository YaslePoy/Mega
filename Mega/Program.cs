using Mega.Game.Blocks;
using Mega.Video;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Mega
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Mega game startup code |
            //                        |
            //                        V

            // var nativeWindowSettings = new NativeWindowSettings()
            // {
            //     Size = new Vector2i(1920, 1080),
            //     Title = "Mega Game",
            //     // This is needed to run on macos
            //     Flags = ContextFlags.ForwardCompatible,
            //     WindowBorder = WindowBorder.Hidden,
            //
            //     //WindowState = WindowState.Fullscreen
            // };
            // using (var window = new Window(new GameWindowSettings() /*{ UpdateFrequency = 100 }*/, nativeWindowSettings) { })
            // {
            //     window.Run();
            // }
            
            
            Console.WriteLine("Hello Vulkan!");

            
            var rs = new RenderSurface(Block.MeshSides[4].Reverse().Select(i => i - Vector3.One / 2).ToArray(), new[] { Vector2.Zero, new(0, 1), new(1, 1), new(1, 0) },
                Vector3.Zero, Vector3.UnitZ, 1);


            var arr = new List<RenderSurface> { rs };
            
            ImageResult image;
            using (Stream stream = File.OpenRead("Resources/0.png"))
            {
                image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            }

            OmegaEngine.SetMainRenderTexture(image.Data, image.Width, image.Height);

            OmegaEngine.InitWindow(1000, 700);
            OmegaEngine.Start();

            OmegaEngine.SetMeshShaderData(arr.ToArray(), 1);

            
            while (OmegaEngine.GetWindowCloseState() == 0)
            {
                OmegaEngine.UpdateKeyboardState();
                OmegaEngine.PollWindowEvents();
                OmegaEngine.Draw();
            }
        }
    }
}