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

            TextureHelper.Load();
            Atlas.Main.Assemble();

            var maps = Atlas.Main["Mega:stoneBlock", 0];

            var b = maps;
            Console.WriteLine(b[0]);
            var rs = new RenderSurface(Block.MeshSides[1],
                /*[new(1, 1), new(1, 0), Vector2.Zero, new(0, 1)]*/Atlas.Main["Mega:birchBlock", 1],
                new Vector3(-0.5f, -1, -0.5f), Vector3.UnitY, 1);


            StoneBlock sb = new StoneBlock(Vector3i.Zero);
            var arr = new List<RenderSurface> { rs };


            var mainTex = Atlas.Main.Image;
             // for set first pixel red
            mainTex.data[1] = mainTex.data[2] = 0;
            
            OmegaEngine.SetMainRenderTexture(mainTex.data, mainTex.X, mainTex.Y);
            
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