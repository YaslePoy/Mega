using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public enum OrderMode
    {
        Circle, Z
    }
    public struct LimitedPlane
    {
        public int[] orders;
        public Vector3[] Limits;
        public Plane plane;
        public LimitedPlane(Vector3 normal, Vector3[] limits, OrderMode order = OrderMode.Z)
        {
            plane = new Plane(normal,
                -Vector3.Dot(normal, limits[0]));
            Limits = limits;
            if(order == OrderMode.Circle)
            {
                orders = new []{ 0, 1, 2, 0, 2, 3 };
            }
            else
            {
                orders = new[] { 0, 1, 2, 1, 2, 3 };
            }
        }
        public static implicit operator Plane(LimitedPlane x)
        {
            return x.plane;
        }
        public static LimitedPlane operator +(LimitedPlane x, Vector3 move)
        {
            x.Limits = x.Limits.Select(i => i + move).ToArray();
            return x;
        }
        public bool IsContains(Vector3 point)
        {
            var prePts = new Vector3[Limits.Length + 1];
            prePts[0] = point;
            Limits.CopyTo(prePts, 1);
            var pts = new Vector2[3];
            if (plane.Normal.X != 0)
                pts = prePts.Select(i => i.Yz).ToArray();
            else if (plane.Normal.Y != 0)
                pts = prePts.Select(i => i.Xz).ToArray();
            else
                pts = prePts.Select(i => i.Xy).ToArray();
            var triangles = orders.Chunk(3).ToList();
            foreach (var tri in triangles)
            {
                var points = new Vector2[] { pts[0], pts[tri[0] + 1], pts[tri[1] + 1], pts[tri[2] + 1] };
                if(IsInTreangle(points))
                    return true;
            }
            return false;

        }
        static bool IsInTreangle(Vector2[] pts)
        {
            float a = (pts[1].X - pts[0].X) * (pts[2].Y - pts[1].Y) - (pts[2].X - pts[1].X) * (pts[1].Y - pts[0].Y);
            float b = (pts[2].X - pts[0].X) * (pts[3].Y - pts[2].Y) - (pts[3].X - pts[2].X) * (pts[2].Y - pts[0].Y);
            float c = (pts[3].X - pts[0].X) * (pts[1].Y - pts[3].Y) - (pts[1].X - pts[3].X) * (pts[3].Y - pts[0].Y);
            return (a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0);
        }
    }
}
