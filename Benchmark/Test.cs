using Mega;
using OpenTK.Mathematics;
using System;

namespace Benchmark
{
    internal unsafe class Test
    {
        private Vector3i[] a;
        public unsafe Test()
        {
            var r = new Random();
            var half = int.MaxValue / 2;
            a = new Vector3i[200];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = new Vector3i(r.Next() - half, r.Next() - half, r.Next() - half);
            }
        }

        public void Cs()
        {
            for (int i = 0; i < a.Length; i++)
            {
                var n = a[i].ToWorldPath();
            }
        }
        public void Cpp()
        {
            for (int i = 0; i < a.Length; i++)
            {
                var n = CppHelpers.ToWorldPath(a[i]);
            }
        }


    }
}
