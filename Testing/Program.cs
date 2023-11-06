using Mega;
using OpenTK.Mathematics;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var r = new Random();
            var half = int.MaxValue / 2;
            for (int i = 0; i < 200; i++)
            {
                Vector3i x = new Vector3i(r.Next() - half, r.Next(-5, 200), r.Next() - half);
                var cs = x.InChunk();
                var cpp = CppHelpers.ToWorldPath(x).InChunk;
                Console.Write(cs == cpp);
                Console.WriteLine();

            }
        }
    }
}
