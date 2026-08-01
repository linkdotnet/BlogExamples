using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[SimpleJob(RuntimeMoniker.Net11_0)]
public class DateTimeNowBenchmarks
{
    [Benchmark] public DateTime Now() => DateTime.Now;
}