using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CubicCollider(Vector3 position) : this(position, new bool[] { true, true, true, true, true, true })
        {

        }
        public CubicCollider(Vector3 position, bool[] verify)
        {
            if (position == Vector3.One)
                Console.WriteLine();
            Position = position;
            var limits = Block.MeshSides;
            var normals = Block.Neibs;
            sides = new LimitedPlane[verify.Count(i => i)];
            int j = 0;
            for (int i = 0; i < 6; i++)
            {
                if (verify[i])
                    sides[j++] = OneCube[i] + position;
            }
            var vts = new List<Vector3>();
            sides.ToList().ForEach(side => vts.AddRange(side.Limits));
            Vertexes = vts.Distinct().ToArray();
        }
    }


}
