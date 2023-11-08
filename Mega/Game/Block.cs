using Mega.Video;
using OpenTK.Mathematics;

namespace Mega.Game
{
    public class Block
    {
        public List<RenderSurface> view;
        public readonly Vector3i[] Adjacent;
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
        public int IDCode;
        public Vector3i Position;

        public Block(Vector3i pos, int id, bool generateSurface = true)
        {
            Position = pos;
            IDCode = id;
            var fastPosition = Position.ToFastVector();
            Adjacent = new Vector3i[6];
            Adjacent[0] = Utils.FastAdd(Neibs[0], fastPosition);
            Adjacent[1] = Utils.FastAdd(Neibs[1], fastPosition);
            Adjacent[2] = Utils.FastAdd(Neibs[2], fastPosition);
            Adjacent[3] = Utils.FastAdd(Neibs[3], fastPosition);
            Adjacent[4] = Utils.FastAdd(Neibs[4], fastPosition);
            Adjacent[5] = Utils.FastAdd(Neibs[5], fastPosition);
        }
        public List<RenderSurface> GetDrawingMesh(UnitedChunk area)
        {
            view = new List<RenderSurface>(3);
            List<byte> sides = new List<byte>(3);
            for (byte i = 0; i < 6; i++)
            {
                if (area.GetMember(Adjacent[i]))
                    continue;
                var surface = new RenderSurface(MeshSides[i], TextureHelper.GetTextureCoords(IDCode, i), Position,
                    Neibs[i], IDCode);
                view.Add(surface);
                sides.Add(i);
            }
            collider = new CubicCollider(Position, sides);
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
