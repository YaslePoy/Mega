using Mega.Game;
using Mega.Generation;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public UIShader() : base("Shaders/ui.vert", "Shaders/ui.frag")
        {
        }

        public override void Load()
        {
            base.Load();

            using (Stream stream = File.OpenRead("Shaders\\crist.png"))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                tx = new Texture()
                {
                    width = image.Width,
                    height = image.Height,
                    pixels = image.Data
                };
                tx.Use(TextureUnit.Texture0);
                tx.Update();
            }

            var vertexLocation = GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(float), _indices, BufferUsageHint.DynamicDraw);
        }
        public override void Run(World world)
        {
            BindBuffers();

            //tx.Use(TextureUnit.Texture0);
            TextureHelper.TotalUVMaps[0].tex.Use(TextureUnit.Texture0);
            SetInt("texture0", 0);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
