using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[SimpleJob(RuntimeMoniker.Net11_0)]
[DisassemblyDiagnoser]
public class RangeCheckBenchmarks
{
    private readonly int[] _numbers =
        [.. Enumerable.Range(0, 1024)];

    [Benchmark]
    public int VectorizedSum()
    {
        ReadOnlySpan<int> data = _numbers;
        Vector128<int> sum = default;

        while (data.Length >= Vector128<int>.Count)
        {
            sum += Vector128.Create(data);
            data = data.Slice(Vector128<int>.Count);
        }

        var result = Vector128.Sum(sum);

        foreach (var value in data)
            result += value;

        return result;
    }
}