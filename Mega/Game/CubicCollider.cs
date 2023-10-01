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
        LimitedPlane[] planes;
        public readonly Vector3 Position;

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
            planes = new LimitedPlane[verify.Count(i => i)];
            int j = 0;
            for (int i = 0; i < 6; i++)
            {
                if (verify[i])
                    planes[j++] = new LimitedPlane(normals[i], limits[i].Select(i => i + position).ToArray());
            }
        }
        public override VolumeMembership GetMembership(Vector3 point)
        {
            if(point.IsInRange(Position, Position + Vector3.One))
            {
                if (point.X == MathF.Truncate(point.X) || point.Y == MathF.Truncate(point.Y) || point.Z == MathF.Truncate(point.Z))
                    return VolumeMembership.Border;
                return VolumeMembership.Into;
            }
            return VolumeMembership.Out;
        }

        public override bool MoveToPossible(Vector3 startPt, Vector3 move, out Vector3 nextPosition)
        {
            var checkCol = planes.Where(i => -Vector3.Dot(i.Normal, move) > 0).ToArray();
            nextPosition = startPt;
            foreach (var col in checkCol)
            {
                var t = -((Vector3.Dot(col.Normal, startPt) - col.plane.D) / Vector3.Dot(col.Normal, move));
                if (t < 0 || t > 1)
                    continue;
                if (t != 0)
                    nextPosition = startPt + move * t;
                return true;
            }
            return false;
        }
    }


}
