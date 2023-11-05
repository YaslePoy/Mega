using Mega.Game;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Diagnostics;

namespace Mega.Video.Shading
{
    internal class TextureDrawShader : Shader
    {
        int frames = 0;
        private Dictionary<int, int> _drawOrder;
        public Matrix4 View { set { SetMatrix4("view", value); } }
        public Matrix4 Projection { set { base.SetMatrix4("projection", value); } }

        public Texture RenderTexture
        {
            set
            {
                value.Use(TextureUnit.Texture0);
                SetInt("texture0", 0);
            }
        }
        public TextureDrawShader() : base("Shaders/shader.vert", "Shaders/shader.frag")
        {
        }

        public override void Load()
        {
            base.Load();
            var vertexLocation = GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var texCoordLocation = GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            var norLocation = GetAttribLocation("nor");
            GL.EnableVertexAttribArray(norLocation);
            GL.VertexAttribPointer(norLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            var x = GL.GetUniformLocation(this.Handle, "viewDir");
        }
        public override void Run(World world)
        {
            BindBuffers();
            if (world.Redrawing)
            {
                Stopwatch sw = Stopwatch.StartNew();
                world.GenerateMesh(out Dictionary<int, List<uint>> _indeces, out float[] _vertices);
                sw.Stop();
                Console.WriteLine($"MeshGen {sw.Elapsed}");
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
                var inds = _indeces.Values.ToList().SumList();
                _drawOrder = new Dictionary<int, int>();
                foreach (var order in _indeces)
                {
                    _drawOrder.Add(order.Key, order.Value.Count);
                }
                GL.BufferData(BufferTarget.ElementArrayBuffer, inds.Length * sizeof(float), inds, BufferUsageHint.DynamicDraw);
            }
            Projection = world.Player.Cam.GetProjectionMatrix();
            View = world.Player.Cam.GetViewMatrix();
            int offset = 0;
            foreach (var tex in _drawOrder)
            {
                RenderTexture = TextureHelper.TotalUVMaps[tex.Key].tex;
                var currentDrawArray = tex.Value;
                GL.DrawElements(PrimitiveType.Triangles, currentDrawArray, DrawElementsType.UnsignedInt, offset * sizeof(uint));
                offset += currentDrawArray;
            }
            frames++;
        }
    }
}
