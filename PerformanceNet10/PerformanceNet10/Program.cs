
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(VirtualizationAndInlineBenchmarks).Assembly).Run();

[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
public class VirtualizationAndInlineBenchmarks
{
    private static readonly List<int> Numbers = Enumerable.Range(0, 10000).Select(s => Random.Shared.Next()).ToList();

    // See: https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/runtime
    [Benchmark]
    public int Sum()
    {
        var sum = 0;
        IEnumerable<int> temp = Numbers;
        foreach (var t in temp)
            sum += t;
        
        return sum;
    }

    // See: https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/runtime
    [Benchmark]
    public int StackallocOfArrays()
    {
        int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        var sum = 0;

        for (var i = 0; i < numbers.Length; i++)
            sum += numbers[i];

        return sum;
    }
}

[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
public class CommonListOperationBenchmarks
{
    [Benchmark]
    public List<int> ListAdd10000()
    {
        var list = new List<int>();
        for (var i = 0; i < 10_000; i++)
        {
            list.Add(i);
        }

        return list;
    }

    [Benchmark]
    public List<int> ListAdd10000PreAlloc()
    {
        var list = new List<int>(10_000);
        for (var i = 0; i < 10_000; i++)
        {
            list.Add(i);
        }

        return list;
    }
}