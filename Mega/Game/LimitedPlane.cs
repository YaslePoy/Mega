using OpenTK.Mathematics;

namespace Mega.Game
{
    public enum OrderMode
    {
        Circle, Z
    }
    public struct LimitedPlane
    {
        static readonly int[][] triangles = { new[] { 0, 1, 3 }, new[] { 1, 2, 3 } };
        public Vector3 P1;
        public Vector3 P2;
        public Vector3 P3;
        public Vector3 P4;
        public Vector3[] Limits => new[] { P1, P2, P3, P4 };
        public Plane plane;
        public LimitedPlane(Vector3 normal, Vector3[] limits, OrderMode order = OrderMode.Z)
        {
            plane = new Plane(normal,
                -Vector3.Dot(normal, limits[0]));
            //Limits = limits;
            P1 = limits[0];
            P2 = limits[1];
            P3 = limits[2];
            P4 = limits[3];
        }
        public static implicit operator Plane(LimitedPlane x)
        {
            return x.plane;
        }
        public static LimitedPlane operator +(LimitedPlane x, Vector3 move)
        {
            var moveFast = move.ToFastVector();
            x.P1 = Utils.FastAdd(x.P1, moveFast);
            x.P2 = Utils.FastAdd(x.P2, moveFast);
            x.P3 = Utils.FastAdd(x.P3, moveFast);
            x.P4 = Utils.FastAdd(x.P4, moveFast);

            x.UpdateDRatio();
            return x;
        }
        public static LimitedPlane operator *(LimitedPlane x, Vector3 mul)
        {
            var moveFast = mul.ToFastVector();
            x.P1 = Utils.FastMul(x.P1, moveFast);
            x.P2 = Utils.FastMul(x.P2, moveFast);
            x.P3 = Utils.FastMul(x.P3, moveFast);
            x.P4 = Utils.FastMul(x.P4, moveFast); 
            x.UpdateDRatio();
            return x;
        }
        public void UpdateDRatio()
        {
            plane.D = -Vector3.Dot(plane.Normal, P1);
        }
        public bool IsContains(Vector3 point)
        {
            var prePts = new[] {point, P1, P2, P3, P4};
            var pts = new Vector2[3];
            if (plane.Normal.X != 0)
                pts = prePts.Select(i => i.Yz).ToArray();
            else if (plane.Normal.Y != 0)
                pts = prePts.Select(i => i.Xz).ToArray();
            else
                pts = prePts.Select(i => i.Xy).ToArray();
            foreach (var tri in triangles)
            {
                var points = new[] { pts[0], pts[tri[0] + 1], pts[tri[1] + 1], pts[tri[2] + 1] };
                if (IsInTreangle(points))
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
        public override string ToString()
        {
            return plane.ToString();
        }
    }
}
