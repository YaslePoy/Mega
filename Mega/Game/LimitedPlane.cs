using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using Sys = System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    internal class LimitedPlane
    {
        public Vector3 Normal;
        public Vector3[] Limits;
        public Sys.Plane plane;
        public LimitedPlane(Vector3 normal, Vector3[] limits)
        {
            plane = new Sys.Plane(normal.ToSys(),
                Vector3.Dot(normal, limits[0]));
            Normal = normal;
            Limits = limits;
        }
    }
}
