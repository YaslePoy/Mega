using Mega;
using OpenTK.Mathematics;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Benchmark
{
    internal unsafe class Test
    {
        private Vector3[] a, b;
        private Vector128<float>[] m, n;
        public unsafe Test()
        {
            var r = new Random();
            a = new Vector3[200];
            b = new Vector3[200];
            m = new Vector128<float>[200];
            n = new Vector128<float>[200];
            for (int i = 0; i < a.Length; i++)
            {
                var x = new Vector3(r.NextSingle(), r.NextSingle(), r.NextSingle());
                var y = new Vector3(r.NextSingle(), r.NextSingle(), r.NextSingle());
                a[i] = x;
                b[i] = y;
                m[i] = Vector128.Create(x.X, x.Y, x.Z, 0f);
                n[i] = Vector128.Create(y.X, y.Y, y.Z, 0f);
            }
        }

        public void Cs()
        {
            for (int i = 0; i < a.Length; i++)
            {
                var result = a[i] / b[i];
            }
        }
        public void Cpp()
        {
            for (int i = 0; i < a.Length; i++)
            {
                var result = a[i] / b[i];
            }
        }


    }
}
