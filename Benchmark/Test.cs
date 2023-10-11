using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega;
using OpenTK.Mathematics;

namespace Benchmark
{
    internal class Test
    {
        public void a()
        {
            var l1 = new Line(Vector3.UnitZ, new Vector3(1, 1, 0), 1);
            var l2 = new Line(-Vector3.UnitZ, new Vector3(-1, 1, 0), 1);
            var x = GeometricEngine.LinesDistance(l1, l2, false);
        }
        public void fast()
        {
            var l1 = new Line(Vector3.UnitZ, new Vector3(1, 1, 0), 1);
            var l2 = new Line(-Vector3.UnitZ, new Vector3(-1, 1, 0), 1);
            var x = GeometricEngine.LinesDistance(l1, l2);
        }
    }
}
