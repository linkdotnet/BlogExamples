using System.Buffers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ArrayBenchmark>();


[MemoryDiagnoser]
public class ArrayBenchmark
{
    [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int ArraySize { get; set; }

    [Benchmark(Baseline = true)]
    public int[] NewArray() => new int[ArraySize];

    [Benchmark]
    public int[] ArrayPoolRent() => ArrayPool<int>.Shared.Rent(ArraySize);

    [Benchmark]
    public int[] GCZeroInitialized() => GC.AllocateArray<int>(ArraySize);

    [Benchmark]
    public int[] GCZeroUninitialized() => GC.AllocateUninitializedArray<int>(ArraySize);
}