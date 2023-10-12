using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Compute.OpenCL;

namespace Mega
{
    public static class GeometricEngine
    {
        public static Vector3 VectorToPlaneProjection(Vector3 normal, Vector3 vector, bool normalize = false)
        {
            var delta = Vector3.Dot(normal, vector);
            if (normalize)
                delta /= normal.LengthSquared;
            return vector - delta * normal;
        }
        public static Vector3 PlaneAndLine(Plane plane, Line line, out float t)
        {
            t = -((Vector3.Dot(plane.Normal, line.Position) + plane.D) / Vector3.Dot(plane.Normal, line.Direction));
            return line.GetByT(t);
        }
        public static Vector3 PlaneAndLine(Plane plane, Line line)
        {
            return PlaneAndLine(plane, line, out float t);
        }
        public static Vector3 VectorMul(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.Z * b.Y - a.Y * b.X);
        }

        public static Line PerpendicularLineByPoint(Line line, Vector3 point)
        {
            var pl = new Plane(line.Direction, point);
            var pt = PlaneAndLine(pl, line);
            return new Line(point, pt);
        }
        public static float LinesDistance(Line l1, Line l2, bool fast = true)
        {
            Matrix3 upper = new Matrix3() { Row0 = l2.Position - l1.Position, Row1 = l1.Direction, Row2 = l2.Direction };
            var u = upper.Determinant;
            var lowerMt1 = MathF.Pow(new Matrix2() { Row0 = l1.Direction.Xy, Row1 = l2.Direction.Xy }.Determinant, 2);
            var lowerMt2 = MathF.Pow(new Matrix2() { Row0 = l1.Direction.Yz, Row1 = l2.Direction.Yz }.Determinant, 2);
            var lowerMt3 = MathF.Pow(new Matrix2() { Row0 = l1.Direction.Zx, Row1 = l2.Direction.Zx }.Determinant, 2);
            var fullLower = fast ? 1 / MathHelper.InverseSqrtFast(lowerMt1 + lowerMt2 + lowerMt3) : MathF.Sqrt(lowerMt1 + lowerMt2 + lowerMt3);
            return u / fullLower;
        }
    }
    public struct Line
    {
        public Vector3 Position;
        public Vector3 Direction;
        public float Length = 1;
        public Vector3 EndPoint => Position + Direction * Length;
        public Line()
        {
            Position = Vector3.Zero;
            Direction = Vector3.UnitX;
        }
        public Line(Vector3 xyz, Vector3 direction, float length)
        {
            Position = xyz;
            Direction = direction;
            Length = length;
        }
        public Line(Vector3 xyz, Vector3 endPoint)
        {
            Position += xyz;
            Direction = endPoint - xyz;
        }
        public Vector3 GetByT(float t)
        {
            return Position + Direction * t;
        }
    }
    public struct Plane
    {
        public Vector3 Normal;
        public float D;
        public Plane() : this(Vector3.UnitX, 0) { }
        public Plane(Vector3 normal, float d)
        {
            Normal = normal;
            D = d;
        }
        public Plane(Vector3 normal, Vector3 pointIn)
        {
            Normal = normal;
            D = Vector3.Dot(-normal, pointIn);
        }
    }
}
