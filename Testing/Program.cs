using System.Runtime.Intrinsics;
using Mega;
using OpenTK.Mathematics;
using StbImageSharp;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ImageResult image;
            using (Stream stream = File.OpenRead(@"C:\Users\Mimm\Downloads\readTest.png"))
            {
                image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            }

            Console.WriteLine(image.Data);
        }
    }
}
