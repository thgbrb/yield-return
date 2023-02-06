using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace YieldReturn
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkYieldReturn>();
        }
    }

    [SimpleJob(RuntimeMoniker.Net461, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net48)]
    [SimpleJob(RuntimeMoniker.Net50)]
    [SimpleJob(RuntimeMoniker.Net70)]
    [MemoryDiagnoser]
    [RPlotExporter]
    public class BenchmarkYieldReturn
    {
        [Params(100, 1_000, 10_000, 100_000, 1_000_000, 100_000_000)]
        public int Iterations;

        [Benchmark(Baseline = true)]
        public void ConsumeWithouYieldReturn()
        {
            _ = FilteringWithout(Iterations);
        }

        [Benchmark]
        public void ConsumeWithYieldReturn()
        {
            _ = FilterWith(Iterations);
        }

        private IEnumerable<int> FilteringWithout(int iterations)
        {
            var range = Enumerable.Range(1, iterations);
            var evenList = new List<int>();

            foreach (var number in range)
            {
                if (number % 2 == 0)
                    evenList.Add(number);
            }

            return evenList;
        }

        private IEnumerable<int> FilterWith(int iterations)
        {
            var range = Enumerable.Range(1, iterations);

            foreach (var number in range)
            {
                if (number % 2 == 0)
                    yield return number;
            }
        }
    }
}