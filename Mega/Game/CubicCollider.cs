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
            Position = position;
            var limits = Block.MeshSides;
            var normals = Block.Neibs;
            planes = new LimitedPlane[verify.Count(i => i)];
            int j = 0;
            for (int i = 0; i < 6; i++)
            {
                if (verify[i])
                    planes[j++] = new LimitedPlane(normals[i], limits[i].Select(i => i * position).ToArray());
            }
        }
        public override bool IsContains(Vector3 point)
        {
            return point.IsInRange(Position, Position + Vector3.One);
        }

        public override bool MoveToPossible(ref Vector3 nextPt, Vector3 startPt)
        {
            var delta = nextPt - startPt;
            var checkCol = planes.Where(i => Vector3.Dot(i.Normal, delta) > 0).ToArray();
            var deltaLen = delta.LengthSquared;
            foreach (var col in checkCol)
            {
                var mul = col.Normal * delta;
                var t = -(mul.X + mul.Y + mul.Z + col.plane.D);
                var projection = (col.Normal * t + nextPt).Round(2);
                if (IsContains(projection) && MathF.Round(deltaLen, 2) == MathF.Round((projection - nextPt).LengthSquared + (projection - startPt).LengthSquared))
                {
                    nextPt = projection;
                    return true;
                }
            }
            return false;
        }
    }
}
