using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ChunkedList;

BenchmarkRunner.Run<Benchmarks>();

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