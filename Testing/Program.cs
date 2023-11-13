using System.Runtime.Intrinsics;
using Mega;
using OpenTK.Mathematics;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var x = new int[]{1, 2, 3, 4, 5};
            int[] n = CppHelpers.GetAdjacent(Vector3i.One, x, x.Length);
            Console.WriteLine();

        }
    }
}
