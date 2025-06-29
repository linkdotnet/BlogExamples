using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MarkdownExporterAttribute.GitHub]
[KeepBenchmarkFiles]
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

    // See: https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/runtime#stack-allocation
    [Benchmark]
    public int StackallocOfArrays()
    {
        int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        var sum = 0;

        for (var i = 0; i < numbers.Length; i++)
            sum += numbers[i];

        return sum;
    }
    
    // See: https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview5/runtime.md#escape-analysis-for-delegates
    [Benchmark]
    public int DelegateEscapeAnalysis()
    {
        var sum = 0;
        Action<int> action = i => sum += i;

        foreach (var number in Numbers)
            action(number);

        return sum;
    }
}