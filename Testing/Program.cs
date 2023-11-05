using Mega;
using OpenTK.Mathematics;

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //BinaryInt i = new BinaryInt(13, 4);
            //BinaryInt j = new BinaryInt(5, 3);
            //var x = i + j;
            for (int i = 5; i >= -70; i--)
            {

                var testVec = new Vector3i(i, 0, 5);
                var res = testVec.ToWorldPath();
                Console.WriteLine($"{testVec} chunk is {res.chunk} in chunk {res.block}");

            }
        }
    }
}
