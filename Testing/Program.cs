using System.Runtime.Intrinsics;
using Mega;
using OpenTK.Mathematics;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var r = new Random();
            //var half = int.MaxValue / 2;
            //for (int i = 0; i < 200; i++)
            //{
            //    Vector3i x = new Vector3i(r.Next() - half, r.Next(-5, 200), r.Next() - half);
            //    var cs = x.InChunk();
            //    var cpp = CppHelpers.ToWorldPath(x).InChunk;
            //    Console.Write(cs == cpp);
            //    Console.WriteLine();

            //}
            var n = Vector128.Create(5f, 2f, 3f, 4f);
            var m = Vector128.Create(10f, 4f, 6f, 8f);
            var y = n / m;
            Vector128<double> x = new Vector128<double>();
            
        }
    }
}
