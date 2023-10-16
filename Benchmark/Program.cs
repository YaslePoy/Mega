using EasyBenchmark;

namespace Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Runner<Test> runner = new Runner<Test>();
            runner.RunBenchmark(10000);
            Console.ReadLine();
        }
        
    }
}