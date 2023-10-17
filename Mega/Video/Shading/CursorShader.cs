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
        float[] mesh;
        uint[] orders;
        Vector4 Color { set { SetVector4("color", value); } }
        public Matrix4 View { set { SetMatrix4("view", value); } }
        public Matrix4 Projection { set { SetMatrix4("projection", value); } }
        Vector3i cursor;
        public CursorShader() : base("Shaders/cursor.vert", "Shaders/cursor.frag") { }

        public override void Load()
        {
            base.Load();
            Color = new Vector4(1, 1, 1, 1);
            orders = new uint[0];
            var vertexLocation = GetAttribLocation("vPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        }
        public override void Run(World world)
        {
            BindBuffers();
            var worldCursor = world.Player.SelectedBlock;
            if (cursor != worldCursor)
            {
                cursor = worldCursor;
                var cursorBlock = world.Area.GetBlock(cursor);

                if (cursorBlock != null)
                {

                    mesh = cursorBlock.view.Select(i => i.GetRawEdges()).SumList();
                    orders = new uint[cursorBlock.view.Count() * 8];
                    uint offset = 0, index = 0;
                    for (int i = 0; i < cursorBlock.view.Count(); i++, offset+=8, index += 4)
                    {
                        orders[offset] = index;
                        orders[offset + 1] = index + 1;

                        orders[offset + 2] = index + 1;
                        orders[offset + 3] = index + 2;

                        orders[offset + 4] = index + 2;
                        orders[offset + 5] = index + 3;

                        orders[offset + 6] = index + 3;
                        orders[offset + 7] = index;

                    }

                    GL.BufferData(BufferTarget.ArrayBuffer, mesh.Length * sizeof(float), mesh, BufferUsageHint.DynamicDraw);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, orders.Length * sizeof(uint), orders, BufferUsageHint.DynamicDraw);
                }

            }
            GL.DrawArrays(PrimitiveType.Lines, 0, orders.Length);
        }
    }
}
