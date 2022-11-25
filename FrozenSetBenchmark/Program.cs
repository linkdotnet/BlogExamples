using System.Collections.Frozen;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchi>();

public class Benchi
{
    private const int OneHundred = 100;
    private readonly List<int> oneToHundredList = Enumerable.Range(0, OneHundred).ToList();
    private readonly FrozenSet<int> oneToHundredFrozen = Enumerable.Range(0, OneHundred).ToFrozenSet();
    private readonly HashSet<int> oneToHundredHashSet = Enumerable.Range(0, OneHundred).ToHashSet();
    private readonly HashSet<int> oneToHundredImmutableHashSet= Enumerable.Range(0, OneHundred).ToHashSet();

    [Benchmark(Baseline = true)]
    public void LookupList()
    {
        for (var i = 0; i < OneHundred; i++)
            _ = oneToHundredList.Contains(i);
    }

    [Benchmark]
    public void LookupFrozen()
    {
        for (var i = 0; i < OneHundred; i++)
            _ = oneToHundredFrozen.Contains(i);
    }

    [Benchmark]
    public void LookupHashSet()
    {
        for (var i = 0; i < OneHundred; i++)
            _ = oneToHundredHashSet.Contains(i);
    }

    [Benchmark]
    public void LookupImmutableHashSet()
    {
        for (var i = 0; i < OneHundred; i++)
            _ = oneToHundredImmutableHashSet.Contains(i);
    }
}