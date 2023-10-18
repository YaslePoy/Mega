using Mega.Game;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Video.Shading
{
    internal class TestShader : Shader
    {
        private float[] _vertices;
        private Dictionary<int, List<uint>> _indices;
        public Matrix4 View { set { SetMatrix4("view", value); } }
        public Matrix4 Projection { set { base.SetMatrix4("projection", value); } }
        public Texture RenderTexture
        {
            set
            {
                value.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
                SetInt("texture0", 0);
            }
        }
        public TestShader() : base("Shaders/shadertest.vert", "Shaders/shadertest.frag")
        {
        }

        public override void Load()
        {
            base.Load();
            var vertexLocation = GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            _indices = new Dictionary<int, List<uint>>();
            _vertices = new float[0];
        }
        public override void Run(World world)
        {
            BindBuffers();
            //if (world.Redrawing)
            //{
                var sel = world.Area.GetBlock(world.Player.SelectedBlock);
                if (sel is not null)
                {
                    world.generateBlockMesh(out _indices, out _vertices, sel);
                    GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
                    var inds = _indices.Values.ToList().SumList();
                    GL.BufferData(BufferTarget.ElementArrayBuffer, inds.Length * sizeof(float), inds, BufferUsageHint.DynamicDraw);
                }

            //}
            Projection = world.Player.Cam.GetProjectionMatrix();
            View = world.Player.Cam.GetViewMatrix();
            int offset = 0;
            foreach (var tex in _indices)
            {
                RenderTexture = TextureHelper.TotalUVMaps[tex.Key].tex;
                var currentDrawArray = tex.Value;
                for (int i = 0; i < 6; i++)
                {
                    GL.DrawElements(PrimitiveType.LineLoop, 4, DrawElementsType.UnsignedInt, 4*i * sizeof(uint));
                }


                offset += currentDrawArray.Count;
            }
        }
    }
}
