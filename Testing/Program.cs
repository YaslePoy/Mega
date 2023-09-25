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
            //TextureHelper.LoadUV();
            //var w = Chunk.Flat(1, null);
            //w.SetBlock(Vector3i.One * 2, 0);
            BitArray ba = new BitArray(new int[] {5});
            foreach (bool i in ba)
            {
                Console.WriteLine(i);
            }
        }
    }
}
