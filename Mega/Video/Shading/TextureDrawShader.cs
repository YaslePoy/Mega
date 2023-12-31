using Mega.Game;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace Mega.Video.Shading
{
    internal class TextureDrawShader : Shader
    {
        int frames = 0;

        public override void Run()
        {
            OmegaEngine.world.Player.Cam.Apply();
                if (OmegaEngine.world.Redrawing)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    OmegaEngine.world.UpdateTotalMesh();
                    sw.Stop();
                    Console.WriteLine($"Mesh update {sw.Elapsed}");
                }

            frames++;
        }
    }
}