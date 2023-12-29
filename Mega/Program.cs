using Mega.Game;
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
 

            var p1 = new RenderSurface(Block.MeshSides[1], Atlas.Main["Mega:birchBlock", 0],
                new Vector3(-0.5f, -1, -0.5f), Vector3.UnitY, 1);
            var p2 = new RenderSurface(Block.MeshSides[1], Atlas.Main["Mega:birchBlock", 0],
                new Vector3(-0.5f, -1, -0.5f), Vector3.UnitY, 1);

            var sb = new BirchBlock(Vector3i.Zero);
            var arr = new List<RenderSurface> { p1, p2};
            arr = sb.GetDrawingMesh(new UnitedChunk());
            
            var mainTex = Atlas.Main.Image;
            
            OmegaEngine.SetMainRenderTexture(mainTex.data, mainTex.X, mainTex.Y);
            
            OmegaEngine.InitWindow(1000, 700);
            OmegaEngine.Start();

            OmegaEngine.SetMeshShaderData(arr.ToArray(), (uint)arr.Count);


            while (OmegaEngine.GetWindowCloseState() == 0)
            {
                OmegaEngine.UpdateKeyboardState();
                OmegaEngine.PollWindowEvents();
                OmegaEngine.Draw();
            }
        }
    }
}