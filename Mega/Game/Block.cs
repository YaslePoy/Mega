using Mega.Video;
using OpenTK.Mathematics;

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

        public Block()
        {
        }

        public Block(Vector3i pos, int id, bool generateSurface = true)
        {
            Position = pos;
            IDCode = id;
        }

        public Vector3i[] GenerateNeis()
        {
            var result = new Vector3i[6];

            result[0] = Position + Neibs[0];
            result[1] = Position + Neibs[1];
            result[2] = Position + Neibs[2];
            result[3] = Position + Neibs[3];
            result[4] = Position + Neibs[4];
            result[5] = Position + Neibs[5];

            view = Array.Empty<RenderSurface>();
            return result;
        }
        public RenderSurface[] GetDrawingMesh(UnitedChunk area)
        {

            var localBorder = Adjacent;
            List<RenderSurface> surfaces = new List<RenderSurface>(6);
            var sides = new bool[6];
            for (int i = 0; i < localBorder.Count(); i++)
            {
                var m = area.GetMember(localBorder[i]);
                if (m)
                    continue;
                sides[i] = !m;
                var surface = new RenderSurface(MeshSides[i], TextureHelper.GetTextureCoords(IDCode, i), Position,
                    Neibs[i], IDCode);
                surfaces.Add(surface);
            }
            collider = new CubicCollider(Position, sides);
            view = surfaces.ToArray();
            return view;
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
