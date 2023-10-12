using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public abstract class Collider
    {
        public LimitedPlane[] sides;
        public Vector3[] Vertexes;
        public Line[] Edges;
        public Vector3 move;
        public Line[] GetMoveLines()
        {
            return Vertexes.AsParallel().Select(x => new Line(x, move, 1)).ToArray();
        }
        public LimitedPlane[] GetMovePlanes()
        {
            return Edges.AsParallel().Select(x =>
            {
                return new LimitedPlane(GeometricEngine.VectorMul(x.Direction, move),
                    new[] { x.Position, x.EndPoint, x.Position + move, x.EndPoint + move });
            }).ToArray();
        }

        bool PlaneLinesCollision(Collider obstacle, out Vector3 avalibleMove, out Vector3 residualMove)
        {
            avalibleMove = move; residualMove = Vector3.Zero;
            var verifyPlanes = obstacle.sides.Where(i => Vector3.Dot(i.plane.Normal, move) < 0).ToArray();
            var rays = GetMoveLines();
            float bufT = 0, maxT = 1;
            Vector3 collision = Vector3.One, normal = Vector3.One;
            bool isColled = false;
            void Direct()
            {
                foreach (var ray in rays)
                {
                    foreach (var plane in verifyPlanes)
                    {

                        var xyz = GeometricEngine.PlaneAndLine(plane, ray, out bufT);
                        if (bufT < 0 || bufT > 1 || bufT < maxT)
                            continue;
                        if (!plane.IsContains(xyz))
                            continue;
                        maxT = bufT;
                        collision = xyz;
                        normal = plane.plane.Normal;
                        isColled = true;
                    }
                }
            }
            void Revercive()
            {
                foreach (var ray in rays)
                {
                    foreach (var plane in verifyPlanes)
                    {

                        var xyz = GeometricEngine.PlaneAndLine(plane, ray, out bufT);
                        if (bufT < 0 || bufT > 1 || bufT < maxT)
                            continue;
                        if (!plane.IsContains(xyz))
                            continue;
                        maxT = bufT;
                        collision = xyz;
                        normal = plane.plane.Normal;
                        isColled = true;
                    }
                }
            }
            Direct();
            verifyPlanes = sides.Where(i => Vector3.Dot(i.plane.Normal, move) > 0).ToArray();
            obstacle.move = -move;
            rays = obstacle.GetMoveLines();
            Revercive();
            if (!isColled)
                return false;
            avalibleMove = move * maxT;
            var remains = 1 - maxT;
            var moveRemains = move * remains;
            var projection = GeometricEngine.VectorToPlaneProjection(normal, moveRemains);
            residualMove = projection;
            return true;
        }

        public virtual void Collide(Collider constantColider, out Vector3 moved, out Vector3 residualMove)
        {
            moved = move;
            residualMove = Vector3.Zero;
            PlaneLinesCollision(constantColider, out moved, out residualMove);
        }
        //public abstract VolumeMembership GetMembership(Vector3 point);
        //public abstract bool MoveToPossible(Vector3 startPosition, Vector3 moveVector, out Vector3 nextPosition);
        //public enum VolumeMembership
        //{
        //    Out, Border, Into
        //}
        //public abstract SuperCollider ToSuper();
    }
}
