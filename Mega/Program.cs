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

            var sb = new BirchBlock(Vector3i.Zero);
            var arr = sb.GetDrawingMesh(new UnitedChunk());
            sb.Position.X = 1;
            arr.AddRange(sb.GetDrawingMesh(new UnitedChunk()));
            var mainTex = Atlas.Main.Image;

            var cam = new Camera(new Vector3(0, -3, 0), 1);
            cam.Apply();
            OmegaEngine.SetMainRenderTexture(mainTex.data, mainTex.X, mainTex.Y);
            
            OmegaEngine.InitWindow(1000, 700);
            OmegaEngine.Start();

            OmegaEngine.SetMeshShaderData(arr.ToArray(), (uint)arr.Count);


            while (OmegaEngine.GetWindowCloseState() == 0)
            {
                OmegaEngine.UpdateKeyboardState();
                OmegaEngine.PollWindowEvents();

                if (OmegaEngine.IsKeyDown((int)GLFWKeys.D))
                    cam.Position.X += 0.0005f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.A))
                    cam.Position.X -= 0.0005f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.W))
                    cam.Position.Y += 0.0005f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.S))
                    cam.Position.Y -= 0.0005f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.E))
                    cam.Position.Z += 0.0005f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.Q))
                    cam.Position.Z -= 0.0005f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.Right))
                    cam.Yaw -= 0.01f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.Left))
                    cam.Yaw += 0.01f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.Up))
                    cam.Pitch += 0.01f;
                if (OmegaEngine.IsKeyDown((int)GLFWKeys.Down))
                    cam.Pitch -= 0.01f;
                cam.Apply();
                OmegaEngine.Draw();
            }
        }
    }
}