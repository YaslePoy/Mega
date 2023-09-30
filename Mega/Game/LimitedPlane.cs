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
            var fp = limits[0] * normal;
            plane = new Sys.Plane(normal.ToSys(), fp.X + fp.Y + fp.Z);
            Normal = normal;
            Limits = limits;
        }
    }
}
