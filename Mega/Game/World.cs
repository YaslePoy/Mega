using Mega.Video;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    internal class World
    {
        public static readonly Vector3i Size = new Vector3i(32, 32, 64);
        Block[,,] worldData;
        public bool[,,] Border;
        public bool[,,] Members;
        public bool[,,] BorderMembers;
        public List<Vector3i> MembersList;
        public List<Vector3i> BorderMembersList;


        public float[] RawVertexes;
        public uint[] RawOrder;
        public RenderSurface[] Surface;
        List<(int x, int y, int z)> notAir;
        Window view;
        public World(Window window)
        {
            worldData = new Block[Size.X, Size.Y, Size.Z];
            view = window;

            Border = new bool[Size.X, Size.Y, Size.Z];
            Members = new bool[Size.X, Size.Y, Size.Z];
            BorderMembers = new bool[Size.X, Size.Y, Size.Z];
            MembersList = new List<Vector3i>();
            BorderMembersList = new List<Vector3i>();
        }
        public void SetBlock(Vector3i location)
        {
            var block = new Block(location, "birch");
            if (Members.Get(location))
                return;

            worldData.Set(location, block);

            add(block);
            RebuildMesh();
        }

        void add(Block block)
        {

            MembersList.Add(block.Position);
            Members.Set(block.Position, true);

            Border.Set(block.Position, false);
            BorderMembers.Set(block.Position, true);
            BorderMembersList.Add(block.Position);
            var addingBorder = block.CurrentNeibs();

            TrySetBorder(addingBorder[0]);
            TrySetBorder(addingBorder[1]);
            TrySetBorder(addingBorder[2]);
            TrySetBorder(addingBorder[3]);
            TrySetBorder(addingBorder[4]);
            TrySetBorder(addingBorder[5]);

        }

        void TrySetBorder(Vector3i location)
        {
            if (Members.Get(location))
                return;
            Border.Set(location, true);
            if (BorderMembers.Get(location))
            {
                BorderMembersList.Remove(location);
            }
        }
        public void RebuildMesh()
        {
            var sides = new List<RenderSurface>();
            foreach (var borderBlock in BorderMembersList)
            {
                sides.AddRange(worldData.Get(borderBlock).GetDrawingMesh(this));
            }
            int offset = 0;
            uint indOffset = 0;
            var vertexArray = new float[sides.Count * 20];
            var order = new uint[sides.Count * 6];
            for (int i = 0; i < sides.Count; i++)
            {
                var v = sides[i].GetRaw();
                v.CopyTo(vertexArray, 20 * i);
                order[offset + 0] = 0 + indOffset;
                order[offset + 1] = 1 + indOffset;
                order[offset + 2] = 3 + indOffset;
                order[offset + 3] = 1 + indOffset;
                order[offset + 4] = 2 + indOffset;
                order[offset + 5] = 3 + indOffset;

                offset += 6;
                indOffset += 4;
            }
            view?.UpdateMesh(vertexArray, order);
            Surface = sides.ToArray();
            RawOrder = order;
            RawVertexes = vertexArray;
        }

        public Block Get(Vector3i pos)
        {
            return worldData.Get(pos);
        }
    }
}
