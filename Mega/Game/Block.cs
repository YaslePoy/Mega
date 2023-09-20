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

    public enum BlockType
    {
        Air, Wood
    }
    public class Block
    {
        private readonly Vector3i _noNeib = -Vector3i.One;
        Vector3i[] localNeibs;
        Vector3i posibleNeibs;
        public Vector3i[] LocalNeibs => localNeibs == null ? GenerateNeis() : localNeibs;
       
        public static readonly Vector3i[] Neibs = {
            Vector3i.UnitX, Vector3i.UnitY, Vector3i.UnitZ,
            -Vector3i.UnitX, -Vector3i.UnitY, -Vector3i.UnitZ
        };
        public static readonly Vector3[][] MeshSides = {
            new[] { new (1, 0, 0), new (1, 0, 1), new (1, 1, 1), new Vector3(1, 1, 0) },
            new[] { new (0, 1, 0), new (0, 1, 1), new (1, 1, 1), new Vector3(1, 1, 0) },
            new[] { new (0, 0, 1), new (1, 0, 1), new (1, 1, 1), new Vector3(0, 1, 1) },
            new[] { new (0, 0, 0), new (0, 0, 1), new (0, 1, 1), new Vector3(0, 1, 0) },
            new[] { new (0, 0, 0), new (1, 0, 0), new (1, 0, 1), new Vector3(0, 0, 1) },
            new[] { new (0, 0, 0), new (1, 0, 0), new (1, 1, 0), new Vector3(0, 1, 0) }
        };

        public int ID;
        public Vector3i Position;
        public BlockType Type;

        RenderSurface[] totalSurface;
        public Block()
        {
            Type = BlockType.Air;
            totalSurface = new RenderSurface[0];
        }

        public Block(Vector3i pos, int id)
        {
            Type = BlockType.Wood;
            Position = pos;
            ID = id;

            GenerateSurface();
        }

        void GenerateSurface()
        {
            totalSurface = new RenderSurface[6];
            totalSurface[0] = new RenderSurface(MeshSides[0], TextureHelper.GetTextureCoords(ID, 0), Position, ID);
            totalSurface[1] = new RenderSurface(MeshSides[1], TextureHelper.GetTextureCoords(ID, 1), Position, ID);
            totalSurface[2] = new RenderSurface(MeshSides[2], TextureHelper.GetTextureCoords(ID, 2), Position, ID);
            totalSurface[3] = new RenderSurface(MeshSides[3], TextureHelper.GetTextureCoords(ID, 3), Position, ID);
            totalSurface[4] = new RenderSurface(MeshSides[4], TextureHelper.GetTextureCoords(ID, 4), Position, ID);
            totalSurface[5] = new RenderSurface(MeshSides[5], TextureHelper.GetTextureCoords(ID, 5), Position, ID);
        }

        public Vector3i[] GenerateNeis()
        {
            List<Vector3i> result = new List<Vector3i>();
            var neib = Position + Neibs[0];

            result.Add(neib);
            neib = Position + Neibs[1];
            result.Add(neib);
            neib = Position + Neibs[2];
            result.Add(neib);
            neib = Position + Neibs[3];
            result.Add(neib);
            neib = Position + Neibs[4];
            result.Add(neib);
            neib = Position + Neibs[5];
            result.Add(neib);

            return result.ToArray();
        }
        public List<RenderSurface> GetDrawingMesh(Chunk world)
        {
            var localBorder = LocalNeibs;
            List<RenderSurface> surfaces = new List<RenderSurface>();
            for (int i = 0; i < localBorder.Count(); i++)
            {
                    if (!localBorder[i].IsInRange(0, Chunk.Size.X))
                        continue;
                if (world.Members.Get(localBorder[i]))
                    continue;
                surfaces.Add(totalSurface[i]);
            }
            return surfaces;
        }
        public override string ToString()
        {
            return Type.ToString() + $"[{Position.X} {Position.Y} {Position.Z}]";
        }
    }
}
