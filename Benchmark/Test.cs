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
        Dictionary<Vector3, float> dots;
        float[] dotsArray;
        Vector3 a, b, c;
        public Test()
        {
            dots = new Dictionary<Vector3, float>();
            dots.Add(Vector3.UnitX, 1);
            dots.Add(Vector3.UnitY, 2);
            dots.Add(Vector3.UnitZ, 3);
            dots.Add(-Vector3.UnitX, 4);
            dots.Add(-Vector3.UnitY, 5);
            dots.Add(-Vector3.UnitZ, 6);
            dotsArray = new float[] { 1, 2, 3, 4, 5, 6 };
            a = Vector3.UnitX;
            b = -Vector3.UnitY;
            c = new Vector3(1, 2, 3);
        }

        public void dictionary()
        {
            var a1 = dots[a];
            var a2 = dots[b];
            var sum = a1 + a2;
        }
        public void array()
        {
            var a = dotsArray[0];
            var b = dotsArray[4];
            var sum = b + a;
        }

        public void calc()
        {
            var a1 = Vector3.Dot(a, c);
            var a2 = Vector3.Dot(b, c);
            var sum = a1 + a2;
        }
    }
}
