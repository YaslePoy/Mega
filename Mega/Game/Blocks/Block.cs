using System.Reflection;
using Mega.Video;
using OpenTK.Mathematics;

namespace Mega.Game.Blocks
{
    public abstract class Block
    {
        public List<RenderSurface> view;
        public readonly Vector3i[] Adjacent;
        protected CubicCollider collider;

        public static readonly Vector3i[] Neibs =
        {
            Vector3i.UnitX, Vector3i.UnitY, Vector3i.UnitZ,
            -Vector3i.UnitX, -Vector3i.UnitY, -Vector3i.UnitZ
        };

        public static readonly Vector3[][] MeshSides =
        {
            [new(1, 0, 0), new(1, 1, 0),new(1, 1, 1), new(1, 0, 1), ],
            [new(1, 1, 0),new(0, 1, 0), new(0, 1, 1), new(1, 1, 1), ],
            [new(0, 0, 1), new(1, 0, 1), new(1, 1, 1), new(0, 1, 1),],
            
            [new(0, 1, 0), new(0, 0, 0), new(0, 0, 1), new(0, 1, 1),],
            [new(0, 0, 0), new(1, 0, 0), new(1, 0, 1), new(0, 0, 1),],
            [new(0, 1, 0), new(1, 1, 0), new(1, 0, 0), new(0, 0, 0),]
        };

        public readonly int Id;
        public readonly string Code;
        public Vector3i Position;

        public Block(Vector3i pos, int id, string code, string pack="Mega")
        {
            Id = id;
            Position = pos;
            var fastPosition = Position.ToFastVector();
            Adjacent = new Vector3i[6];
            Adjacent[0] = Utils.FastAdd(Neibs[0], fastPosition);
            Adjacent[1] = Utils.FastAdd(Neibs[1], fastPosition);
            Adjacent[2] = Utils.FastAdd(Neibs[2], fastPosition);
            Adjacent[3] = Utils.FastAdd(Neibs[3], fastPosition);
            Adjacent[4] = Utils.FastAdd(Neibs[4], fastPosition);
            Adjacent[5] = Utils.FastAdd(Neibs[5], fastPosition);
            Code = $"{pack}:{code}";
        }

        public abstract List<RenderSurface> GetDrawingMesh(UnitedChunk area);

        public virtual Collider GetCollider()
        {
            return collider;
        }

        public override string ToString()
        {
            return $"[{Position.X} {Position.Y} {Position.Z}]";
        }

        public virtual BinaryInt GetSaveData()
        {
            return new BinaryInt(Id + 2, 4);
        }
    }
}