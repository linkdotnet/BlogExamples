using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ListBenchmarks>();

[MemoryDiagnoser]
public class ListBenchmarks
{
    [Params(1000, 20_0000)] public int Items { get; set; }
    
    [Benchmark(Baseline = true)]
    public List<int> List()
    {
        var list = new List<int>();
        for (var i = 0; i < Items; i++)
        {
            list.Add(i);
        }

        return list;
    }
    
    [Benchmark]
    public ChunkedList<int> ChunkedList()
    {
        var list = new ChunkedList<int>();
        for (var i = 0; i < Items; i++)
        {
            list.Add(i);
        }

        return list;
    }
}

public class ForBenchmark
{
    private readonly List<int> _list = new List<int>();
    private readonly ChunkedList<int> _chunkedList = new ChunkedList<int>();

    [GlobalSetup]
    public void Setup()
    {
        _list.AddRange(Enumerable.Range(0, 20_000));
        foreach (var item in _list)
        {
            _chunkedList.Add(item);
        }
    }
    
    [Benchmark(Baseline = true)]
    public int ForList()
    {
        var sum = 0;
        for (var i = 0; i < _list.Count; i++)
        {
            sum += _list[i];
        }

        return sum;
    }
    
    [Benchmark]
    public int ForChunkedList()
    {
        var sum = 0;
        for (var i = 0; i < _chunkedList.Count; i++)
        {
            sum += _chunkedList[i];
        }

        return sum;
    }
}