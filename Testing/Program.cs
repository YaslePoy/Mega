using Mega.Game;
using Mega.Video;
using OpenTK.Mathematics;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextureHelper.LoadUV();
            Ray r = new Ray(Vector3.Zero, new Vector3(20, 6, 0));
            foreach (var item in r.GetCrossBlocks(21f))
            {
                Console.WriteLine($"Block: {item.block}; Side: {item.side}");
            }
        }
    }
}
