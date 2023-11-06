using EasyBenchmark;

namespace Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Runner<Test> runner = new Runner<Test>();
            runner.RunBenchmark();
            Console.ReadLine();
        }

    }
}