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
            var w = World.GenerateFlat(1, null);
            w.SetBlock(Vector3i.One * 2, 0);
        }
    }
}
