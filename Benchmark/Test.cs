using Mega;
using OpenTK.Mathematics;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Benchmark
{
    internal unsafe class Test
    {
        private Vector3i point = new(750, -7, 23);

        public void Cs()
        {
            for (int i = 0; i < 100; i++)
                point.ToWorldPath();
        }


    }
}
