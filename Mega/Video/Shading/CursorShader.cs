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

            var vertexLocation = GetAttribLocation("vPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        }
        public override void Run(World world)
        {
            BindBuffers();
            var worldCursor = world.Player.Cursor;
            if(cursor != worldCursor)
            {
                cursor = worldCursor;
                var cursorBlock = world.Area.GetBlock(cursor);

                if(cursorBlock != null)
                {
                     mesh = cursorBlock.view.Select(i => i.GetRawEdges()).SumList();
                     orders = new uint[mesh.Length * 2 - 2];
                    for (uint i = 0; i < mesh.Length - 1;)
                    {
                        orders[i] = i;
                        orders[i++] = i;
                    }

                    GL.BufferData(BufferTarget.ArrayBuffer, mesh.Length * sizeof(float), mesh, BufferUsageHint.DynamicDraw);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, orders.Length * sizeof(float), orders, BufferUsageHint.DynamicDraw);
                }

            }
            GL.DrawElements(BeginMode.Lines, orders.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
