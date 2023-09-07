using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using System;

namespace Benchmark
{
    public class Benchmarks
    {
        [Benchmark(Baseline = true)]
        public void Scenario1()
        {
            // Implement your benchmark here
            Block b = new Block(OpenTK.Mathematics.Vector3i.One, "");
            var x = b.LocalNeibs;
        }

        [Benchmark]
        public void Scenario2()
        {
            // Implement your benchmark here
            Block b = new Block(OpenTK.Mathematics.Vector3i.One, "");
            var x = b.GenerateNeisByCode();
        }
    }
}
