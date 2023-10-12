using Mega;
using Mega.Game;
using Mega.Video;
using OpenTK.Mathematics;
using System.Collections;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var n = new Vector3(1, 0, 3);
            var vec = new Vector3(-1, 4, 2);
            var newVec = GeometricEngine.VectorToPlaneProjection(n, vec, true);
            Console.WriteLine(newVec);
        }
    }
}
