using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ArrayAdd>();

public class ArrayAdd
{
private static readonly int[] Numbers = Enumerable.Repeat(1, 512).ToArray();

[Benchmark(Baseline = true)]
public int GetSumNaive()
{
    var result = 0;
    for (var index = 0; index < Numbers.Length; index++)
    {
        var i = Numbers[index];
        result += i;
    }

    return result;
}

[Benchmark]
public int SumLINQ() => Numbers.Sum();

[Benchmark]
public int SumPLINQ() => Numbers.AsParallel().Sum();


[Benchmark]
public int SIMDVectors()
{
    // This depends on your hardware
    // It basically says on how many entries can we performance this single operation
    // aka how big is "multiple"
    var vectorSize = Vector<int>.Count;
    var accVector = Vector<int>.Zero;

    // We are tiling the original list by the hardware vector size
    for (var i = 0; i <= Numbers.Length - vectorSize; i += vectorSize)
    {
        var v = new Vector<int>(Numbers, i);
        accVector = Vector.Add(accVector, v);
    }

    // Scalar-Product of our vector against the Unit vector is its sum
    var result = Vector.Dot(accVector, Vector<int>.One);
    return result;
}
}