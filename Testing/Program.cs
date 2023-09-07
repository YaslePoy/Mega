using Mega.Game;
using OpenTK.Mathematics;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VisualData.LoadVisualData();
            World w = new World(null);
            w.SetBlock(Vector3i.One);
            w.SetBlock(new Vector3i(1, 1, 2));
            w.SetBlock(new Vector3i(1, 2, 2));

        }
    }
}
