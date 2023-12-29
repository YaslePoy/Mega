using Mega.Game;
using OpenTK.Graphics.OpenGL;
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
                OmegaEngine.world.GetTotalMesh();
                sw.Stop();
            }
            frames++;
        }
    }
}
