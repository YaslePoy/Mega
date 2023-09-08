using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public class Ray
    {
        public Vector3 Position;
        public Vector3 Direction;

        public Ray(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction.Normalized();
        }
        //public Vector3i[] GetBlocks(float len)
        //{
        //    Vector3i last = Position.Round();
        //    List<Vector3i> result = new List<Vector3i>() { last };
        //    for (float i = 0; i < len; i += 0.25f) {
        //        var newPos = (last + Direction * i).Round();
        //        if (newPos == last)
        //            continue;
        //        result.Add(newPos);
        //        last = newPos;
        //    }
        //    return result.ToArray();
        //}

        public IEnumerable<(Vector3i block, Vector3i side)> GetCrossBlocks(float rayLen)
        {
            var lenSq = rayLen * rayLen;
            var endPoint = Position + Direction * rayLen;
            var endBlock = endPoint.Round();
            var startBlock = Position.Round();
            Vector3 adding = new Vector3(0.5f, 0.5f, 0.5f);
            var niobous = new List<Vector3i>
            {
                float.Sign(Direction.X) * Vector3i.UnitX,
                float.Sign(Direction.Y) * Vector3i.UnitY,
                float.Sign(Direction.Z) * Vector3i.UnitZ
            };
            niobous = niobous.Where(i => i != Vector3i.Zero).ToList();
            while ((startBlock - Position).LengthSquared < lenSq)
            {
                var lengths = new List<(Vector3i point, float dot, Vector3i add)>();
                foreach (var nib in niobous)
                {
                    var newBlock = startBlock + nib;
                    var vec = newBlock + adding - Position; 
                    vec.Normalize();
                    lengths.Add(((Vector3i point, float dot, Vector3i add))(newBlock, Vector3.Dot(vec, Direction), nib));
                }
                var max = lengths.MaxBy(i => i.dot);
                yield return (max.point, max.add);
                startBlock = max.point;
            }
        }
    }
}
