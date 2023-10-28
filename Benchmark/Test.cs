using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega;
using OpenTK.Mathematics;

namespace Benchmark
{
    internal unsafe class Test
    {
        Vector3 a, b;
        float* cache;
        public unsafe Test()
        {
            a = new Vector3(1, 2, 3);
            b = new Vector3(2, 3, 4);
            cache = new float[] { 1, 2, 3, 4 };
        }

        public unsafe void now()
        {
            var sum = lists.SumList();
        }
        public void next()
        {
        }


    }
}
