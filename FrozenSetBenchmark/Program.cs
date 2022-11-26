using System.Collections.Frozen;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(LookupBenchmark).Assembly).Run();

public class LookupBenchmark
{
    private const int Iterations = 1000;
    private readonly List<int> list = Enumerable.Range(0, Iterations).ToList();
    private readonly FrozenSet<int> frozenSet = Enumerable.Range(0, Iterations).ToFrozenSet();
    private readonly HashSet<int> hashSet = Enumerable.Range(0, Iterations).ToHashSet();
    private readonly ImmutableHashSet<int> immutableHashSet= Enumerable.Range(0, Iterations).ToImmutableHashSet();

    [Benchmark(Baseline = true)]
    public void LookupList()
    {
        for (var i = 0; i < Iterations; i++)
            _ = list.Contains(i);
    }

    [Benchmark]
    public void LookupFrozen()
    {
        for (var i = 0; i < Iterations; i++)
            _ = frozenSet.Contains(i);
    }

    [Benchmark]
    public void LookupHashSet()
    {
        for (var i = 0; i < Iterations; i++)
            _ = hashSet.Contains(i);
    }

    [Benchmark]
    public void LookupImmutableHashSet()
    {
        for (var i = 0; i < Iterations; i++)
            _ = immutableHashSet.Contains(i);
    }
}

public class CreateBenchmark
{
    private readonly int[] from = Enumerable.Range(0, 1000).ToArray();

    [Benchmark(Baseline = true)]
    public List<int> CreateList() => from.ToList();

    [Benchmark]
    public FrozenSet<int> CreateFrozenList() => from.ToFrozenSet();

    [Benchmark]
    public HashSet<int> CreateHashSet() => from.ToHashSet();

    [Benchmark]
    public ImmutableHashSet<int> CreateImmutableHashSet() => from.ToImmutableHashSet();
}