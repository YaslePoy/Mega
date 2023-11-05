using OpenTK.Mathematics;

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
                    lengths.Add((newBlock, Vector3.Dot(vec, Direction), nib));
                }
                var max = lengths.MaxBy(i => i.dot);
                yield return (max.point, max.add);
                startBlock = max.point;
            }
        }
    }
}
