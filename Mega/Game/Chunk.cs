using Mega.Video;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public class Chunk
    {
        public static readonly Vector3i Size = new Vector3i(32, 256, 32);
        public Block[,,] data;
        public bool[,,] Border;
        public bool[,,] Members;
        public bool[,,] BorderMembers;
        public List<Vector3i> MembersList;
        public List<Vector3i> BorderMembersList;
        public readonly Vector2i Location;
        public UnitedChunk Root;

        public RenderSurface[] Surface;
        public Chunk()
        {

            data = new Block[Size.X, Size.Y, Size.Z];
            Border = new bool[Size.X, Size.Y, Size.Z];
            Members = new bool[Size.X, Size.Y, Size.Z];
            BorderMembers = new bool[Size.X, Size.Y, Size.Z];
            MembersList = new List<Vector3i>();
            BorderMembersList = new List<Vector3i>();

        }

        public Chunk(Vector2i location) : this()
        {
            this.Location = location;
        }
        public static Chunk Flat(int level, Vector2i location)
        {
            Chunk result = new Chunk(location);
            var chunkMove = location * Size.Xz;
            var chunkEnd = (location + Vector2i.One) * Size.Xz;
            var blocks = new List<Block>();
            for (int i = chunkMove.X; i < chunkEnd.X; i++)
            {
                for (int j = chunkMove.Y; j < chunkEnd.Y; j++)
                {
                    blocks.Add(new Block(new Vector3i(i, level, j), 0));
                }
            }
            result.GenerateFromBlocks(blocks);
            return result;
        }

        public void SetBlock(Block block)
        {
            if (Members.Get(block.Position))
                return;
            data.Set(block.Position, block);
            MembersList.Add(block.Position);
            Members.Set(block.Position, true);

        }
        public void RebuildMesh()
        {
            var sides = new List<RenderSurface>();
            object l = false;
            Parallel.ForEach(MembersList, i =>
            {
                var s = data.Get(i).GetDrawingMesh(Root);
                lock (l)
                {
                    sides.AddRange(s);

                }
            });
            //foreach (var borderBlock in BorderMembersList)
            //{
            //    sides.AddRange(data.Get(borderBlock).GetDrawingMesh(Root));
            //}
            Surface = sides.ToArray();
        }
        public void GenerateFromBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                data.Set(block.Position.InChunk(), block);
                Members.Set(block.Position.InChunk(), true);
                MembersList.Add(block.Position.InChunk());
            }
            //RebuildMesh();
        }
        public Block Get(Vector3i pos)
        {
            return data.Get(pos);
        }
        bool IsSave(Vector3 pos) => ((Vector3i)(pos)).IsInRange(0, Size.X);

        public void ClearInternalData()
        {
            Border = new bool[Size.X, Size.Y, Size.Z];
            Members = new bool[Size.X, Size.Y, Size.Z];
            BorderMembers = new bool[Size.X, Size.Y, Size.Z];
            BorderMembersList.Clear();
        }
        public IEnumerator<Block> GetEnumerator()
        {
            foreach (var block in data)
                yield return block;
        }

        public void UpdateGlobalCoords()
        {
            int xOffset, yOffset;
            if(Location.X >= 0)
            {
                xOffset = Location.X * Size.X;
            }
            else
            {
                xOffset = (Location.X * Size.X) - 1;
            }
            if(Location.Y >= 0)
            {
                yOffset = Location.Y * Size.Z;
            }
            else
            {
                yOffset = (Location.Y * Size.Z) - 1;
            }
            var blocks = MembersList.Select(i => data.Get(i));
            foreach (var item in blocks)
            {
                item.Position.X += xOffset;
                item.Position.Z += yOffset;
            }
        }
        public void GenerateSurface()
        {
            MembersList.ForEach(i => data.Get(i).GenerateSurface());
        }
        public override string ToString()
        {
            return Location.ToString();
        }
    }
}
