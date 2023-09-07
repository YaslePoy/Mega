using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            Direction = direction;
        }
        public Vector3[] GetBlocks(float len)
        {
            List<Vector3> result = new List<Vector3>();
            for (float i = 0; i < len; i += 0.25f) {

            }
        }
    }
}
