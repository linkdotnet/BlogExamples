using System.Collections.Immutable;
using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class AddBenchmark
{
    private readonly ReadOnlyCollection<int> _readOnlyNumbers = Enumerable.Range(0, 1_000).ToList().AsReadOnly();
    private readonly ImmutableArray<int> _immutableArray = Enumerable.Range(0, 1_000).ToImmutableArray();
    private readonly ImmutableList<int> _immutableList = Enumerable.Range(0, 1_000).ToImmutableList();

    private readonly int[] numbersToAdd = { 1, 2, 3, 4, 5, 6, 7, 8 };

    [Benchmark]
    public IReadOnlyList<int> AddElementReadOnlyCollection()
    {
        var l = _readOnlyNumbers.ToList();
        l.AddRange(numbersToAdd);
        return l;
    }

    [Benchmark]
    public ImmutableArray<int> AddElementImmutableArray()
        => _immutableArray.AddRange(numbersToAdd);

    [Benchmark]
    public ImmutableList<int> AddElementImmutableList()
        => _immutableList.AddRange(numbersToAdd);
}