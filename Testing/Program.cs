using System.Runtime.Intrinsics;
using Mega;
using OpenTK.Mathematics;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = CppHelpers.GetAdjacent(Vector3i.One);
            Console.WriteLine();

        }
    }
}
