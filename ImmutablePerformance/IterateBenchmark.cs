using System.Collections.Immutable;
using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;

public class IterateBenchmark
{
    private readonly List<int> _numbers = Enumerable.Range(0, 1_000).ToList();
    private readonly ReadOnlyCollection<int> _readOnlyNumbers = Enumerable.Range(0, 1_000).ToList().AsReadOnly();
    private readonly ImmutableArray<int> _immutableArray = Enumerable.Range(0, 1_000).ToImmutableArray();
    private readonly ImmutableList<int> _immutableList = Enumerable.Range(0, 1_000).ToImmutableList();

    [Benchmark(Baseline = true)]
    public int IterateList()
    {
        var sum = 0;
        for (var i = 0; i < _numbers.Count; i++)
        {
            sum += _numbers[i];
        }

        return sum;
    }

    [Benchmark]
    public int IterateReadOnlyCollection()
    {
        var sum = 0;
        for (var i = 0; i < _readOnlyNumbers.Count; i++)
        {
            sum += _readOnlyNumbers[i];
        }

        return sum;
    }

    [Benchmark]
    public int IterateImmutableArray()
    {
        var sum = 0;
        for (var i = 0; i < _immutableArray.Length; i++)
        {
            sum += _immutableArray[i];
        }

        return sum;
    }

    [Benchmark]
    public int IterateImmutableList()
    {
        var sum = 0;
        for (var i = 0; i < _immutableList.Count; i++)
        {
            sum += _immutableList[i];
        }

        return sum;
    }
}