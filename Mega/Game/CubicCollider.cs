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
            Vertexes = new Vector3[8];

            Vertexes[0] = Position;
            Vertexes[1] = Position + Vector3.UnitX;
            Vertexes[2] = Position + Vector3.UnitY;
            Vertexes[3] = Position + Vector3.UnitZ;
            Vertexes[4] = Position + new Vector3(1, 0, 1);
            Vertexes[5] = Position + new Vector3(0, 1, 1);
            Vertexes[6] = Position + new Vector3(1, 1, 0);
            Vertexes[7] = Position + Vector3.One;
        }

        public CubicCollider(Vector3 position, List<byte> verify)
        {
            Position = position;
            var limits = Block.MeshSides;
            var normals = Block.Neibs;
            sides = new LimitedPlane[verify.Count];
            int j = 0;
            foreach (var sideId in verify)
                sides[j++] = OneCube[sideId] + position;


            GenerateVertexes();
        }
    }


}
