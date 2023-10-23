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
    public class Block
    {
        public RenderSurface[] view;
        Vector3i[] localNeibs;
        public Vector3i[] Adjacent => localNeibs == null ? GenerateNeis() : localNeibs;
        CubicCollider collider;
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
        public virtual string ID => "";
        public int IDCode;
        public Vector3i Position;

        public RenderSurface[] totalSurface;
        public Block()
        {
            totalSurface = new RenderSurface[0];
        }

        public Block(Vector3i pos, int id, bool generateSurface = true)
        {
            Position = pos;
            IDCode = id;
            if (generateSurface)
                GenerateSurface();
        }

        public void GenerateSurface()
        {
            totalSurface = new RenderSurface[6];
            totalSurface[0] = new RenderSurface(MeshSides[0], TextureHelper.GetTextureCoords(IDCode, 0), Position, Neibs[0], IDCode);
            totalSurface[1] = new RenderSurface(MeshSides[1], TextureHelper.GetTextureCoords(IDCode, 1), Position, Neibs[1], IDCode);
            totalSurface[2] = new RenderSurface(MeshSides[2], TextureHelper.GetTextureCoords(IDCode, 2), Position, Neibs[2], IDCode);
            totalSurface[3] = new RenderSurface(MeshSides[3], TextureHelper.GetTextureCoords(IDCode, 3), Position, Neibs[3], IDCode);
            totalSurface[4] = new RenderSurface(MeshSides[4], TextureHelper.GetTextureCoords(IDCode, 4), Position, Neibs[4], IDCode);
            totalSurface[5] = new RenderSurface(MeshSides[5], TextureHelper.GetTextureCoords(IDCode, 5), Position, Neibs[5], IDCode);
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
            view = new RenderSurface[0];
            return result.ToArray();
        }
        public RenderSurface[] GetDrawingMesh(UnitedChunk area)
        {

            var localBorder = Adjacent;
            List<RenderSurface> surfaces = new List<RenderSurface>();
            var sides = new bool[6];
            for (int i = 0; i < localBorder.Count(); i++)
            {
                var m = area.GetMember(localBorder[i]);
                if (m)
                    continue;
                sides[i] = !m;
                surfaces.Add(totalSurface[i]);
            }
            collider = new CubicCollider(Position/*, sides*/);
            view = surfaces.ToArray();
            return view;
            return totalSurface;
        }
        public Collider GetCollider()
        {
            return collider;
        }
        public override string ToString()
        {
            return $"[{Position.X} {Position.Y} {Position.Z}]";
        }
        public BinaryInt GetSaveData()
        {
            return new BinaryInt(IDCode + 2, 4);
        }
    }
}
