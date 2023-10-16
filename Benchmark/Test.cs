using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mega;
using OpenTK.Mathematics;

namespace Benchmark
{
    internal class Test
    {
        int[][] lists;
        public Test()
        {
            lists = Enumerable.Repeat(Enumerable.Range(0, 1000).ToArray(),100).ToArray();
        }

        public void now()
        {
            var sum = lists.SumList();
        }
        public void next()
        {
        }

        
    }
}
