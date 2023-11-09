using Mega.Game.Blocks;
using OpenTK.Mathematics;

namespace Mega.Game
{
    public class CubicCollider : Collider
    {
        public static readonly LimitedPlane[] OneCube = new LimitedPlane[]{
            new LimitedPlane(Vector3.UnitX, Block.MeshSides[0], OrderMode.Circle),
            new LimitedPlane(Vector3.UnitY, Block.MeshSides[1], OrderMode.Circle),
            new LimitedPlane(Vector3.UnitZ, Block.MeshSides[2], OrderMode.Circle),
            new LimitedPlane(-Vector3.UnitX, Block.MeshSides[3], OrderMode.Circle),
            new LimitedPlane(-Vector3.UnitY, Block.MeshSides[4], OrderMode.Circle),
            new LimitedPlane(-Vector3.UnitZ, Block.MeshSides[5], OrderMode.Circle),
        };
        public Vector3 Position;

        public override void GenerateVertexes()
        {
            Vertexes = new Vector3[] {
                Position,
                Position + Vector3.UnitX,
                Position + Vector3.UnitY,
                Position + Vector3.UnitZ,
                Position + new Vector3(1, 0, 1),
                Position + new Vector3(0, 1, 1),
                Position + new Vector3(1, 1, 0),
                Position + Vector3.One,
            };
        }

        public CubicCollider(Vector3 position, List<byte> verify)
        {
            Position = position;
            sides = new LimitedPlane[verify.Count];
            int j = 0;
            for (int i = 0; i < verify.Count; i++)
                sides[j++] = OneCube[verify[i]] + position;

            GenerateVertexes();
        }
    }


}
