using System.Runtime.Intrinsics;
using Mega;
using OpenTK.Mathematics;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Span<int> x = stackalloc int[]{1, 2, 3, 4, 5};
            var ls = new List<int>();
            ls.AddRange(x);
            x[0] = 5;
            ls.AddRange(x);

        }
    }
}
