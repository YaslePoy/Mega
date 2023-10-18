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
    internal class CursorShader : Shader
    {
        private float[] _vertices;
        private Dictionary<int, List<uint>> _indices;
        public Matrix4 View { set { SetMatrix4("view", value); } }
        public Matrix4 Projection { set { base.SetMatrix4("projection", value); } }
        public CursorShader() : base("Shaders/shadertest.vert", "Shaders/shadertest.frag")
        {
        }

        public override void Load()
        {
            base.Load();
            var vertexLocation = GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            //var texCoordLocation = GetAttribLocation("aTexCoord");
            //GL.EnableVertexAttribArray(texCoordLocation);
            //GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
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
                //world.generateBlockMesh(out _indices, out _vertices, sel);
                UpdateEdges(sel, world);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.DynamicDraw);
                var inds = _indices.Values.ToList().SumList();
                GL.BufferData(BufferTarget.ElementArrayBuffer, inds.Length * sizeof(float), inds, BufferUsageHint.DynamicDraw);
            }
            else
            {
                _indices =new Dictionary<int, List<uint>>();
                _vertices = new float[0];
            }
            //}
            Projection = world.Player.Cam.GetProjectionMatrix();
            View = world.Player.Cam.GetViewMatrix();
            int offset = 0;
            foreach (var tex in _indices)
            {
                var currentDrawArray = tex.Value;
                for (int i = 0; i < 6; i++)
                {
                    GL.DrawElements(PrimitiveType.LineLoop, 4, DrawElementsType.UnsignedInt, 4 * i * sizeof(uint));
                }


                offset += currentDrawArray.Count;
            }
        }
        void UpdateEdges(Block block, World w)
        {
            if (block is null)
            {
                _indices = new Dictionary<int, List<uint>>();
                _vertices = new float[0];
                return;
            }
            var sides = block.view.Where(i =>  Vector3.Dot(i.Normal, w.Player.View) < 0).ToArray();
            uint offset = 0;
            _vertices = new float[sides.Length * 12];
            _indices = new Dictionary<int, List<uint>>();
            uint[] adding;
            for (int i = 0; i < sides.Length; i++)
            {
                var side = sides[i];

                var v = side.GetRawEdges();
                v.CopyTo(_vertices, 4 * 3 * i);

                if (!_indices.ContainsKey(side.TextureID))
                    _indices.Add(side.TextureID, new List<uint>());
                adding = new uint[] { offset, 1 + offset, 2 + offset, 3 + offset };
                offset += 4;
                _indices[side.TextureID].AddRange(adding);
            }
        }
    }
}
