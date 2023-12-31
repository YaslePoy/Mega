using Mega.Game.Blocks;
using Vec3 = OpenTK.Mathematics.Vector3;
using Vec3i = OpenTK.Mathematics.Vector3i;
using Vec2 = OpenTK.Mathematics.Vector2;
using Vec2i = OpenTK.Mathematics.Vector2i;
namespace Mega.Game
{
    public class CubicCollider : Collider
    {
        public static readonly LimitedPlane[] OneCube =
        [
            new(Vec3.UnitX, Block.MeshSides[0], OrderMode.Circle),
            new(Vec3.UnitY, Block.MeshSides[1], OrderMode.Circle),
            new(Vec3.UnitZ, Block.MeshSides[2], OrderMode.Circle),
            new(-Vec3.UnitX, Block.MeshSides[3], OrderMode.Circle),
            new(-Vec3.UnitY, Block.MeshSides[4], OrderMode.Circle),
            new(-Vec3.UnitZ, Block.MeshSides[5], OrderMode.Circle)
        ];
        public Vec3 Position;

        public override void GenerateVertexes()
        {
            Vertexes =
            [
                Position,
                Position + Vec3.UnitX,
                Position + Vec3.UnitY,
                Position + Vec3.UnitZ,
                Position + new Vec3(1, 0, 1),
                Position + new Vec3(0, 1, 1),
                Position + new Vec3(1, 1, 0),
                Position + Vec3.One
            ];
        }

        public CubicCollider(Vec3 position, List<byte> verify)
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
