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
    internal class TextureDrawShader : Shader
    {
        int _ebo, _vao, _vbo;

        private float[] _vertices;
        private Dictionary<int, List<uint>> _indices;
        public Matrix4 View { set { base.SetMatrix4("view", value); } }
        public Matrix4 Projection { set { base.SetMatrix4("projection", value); } }
        public Texture RenderTexture
        {
            set
            {
                value.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
                SetInt("texture0", 0);
            }
        }
        public TextureDrawShader() : base("Shaders/shader.vert", "Shaders/shader.frag")
        {
        }

        public void Load()
        {
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);

            var vertexLocation = GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        }
        public override void Run(World world)
        {
            if (world.Redrawing)
            {
                world.GenerateMesh(out _indices, out _vertices);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
                var inds = _indices.Values.ToList().SumList();
                GL.BufferData(BufferTarget.ElementArrayBuffer, inds.Count() * sizeof(uint), inds.ToArray(), BufferUsageHint.DynamicDraw);
            }
            Projection = world.Player.Cam.GetProjectionMatrix();
            View = world.Player.Cam.GetViewMatrix();
            int offset = 0;
            foreach (var tex in _indices)
            {
                RenderTexture = TextureHelper.TotalUVMaps[tex.Key].tex;
                var currentDrawArray = tex.Value;
                GL.DrawElements(PrimitiveType.Triangles, currentDrawArray.Count(), DrawElementsType.UnsignedInt, offset * sizeof(uint));

                offset += currentDrawArray.Count;
            }
        }
    }
}
