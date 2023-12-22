using BenchmarkDotNet.Running;
using TemplatingBenchmarks.Tests;

namespace TemplatingBenchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Run benchmarks
            BenchmarkRunner.Run<StringReplacementBenchmark>();
            BenchmarkRunner.Run<ConditionalBenchmark>();
        }
    }
}
