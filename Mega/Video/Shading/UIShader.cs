using Mega.Game;
using OpenTK.Graphics.OpenGL;

namespace Mega.Video.Shading
{
    public class UIShader : Shader
    {
        Texture tx;
        private readonly float[] _vertices =
            {
            // Position         Texture coordinates
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // bottom right
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // bottom left
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };
        public void SetTextureData(byte[] data, int width, int height)
        {
            tx.pixels = data;
            tx.width = width;
            tx.height = height;
            tx.Update();
        }
        public void SetTextureData(Image img)
        {
            SetTextureData(img.Data, img.Width, img.Height);
        }
       
        public override void Run()
        {
            tx.Use(TextureUnit.Texture0);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
