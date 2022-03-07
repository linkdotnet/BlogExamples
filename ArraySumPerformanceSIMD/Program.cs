using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<ArrayAdd>();

public class ArrayAdd
{
    private int[] numbers = Enumerable.Repeat(1, 512).ToArray();

    [Benchmark]
    public int GetSumNaive()
    {
        var result = 0;
        for (var index = 0; index < numbers.Length; index++)
        {
            var i = numbers[index];
            result += i;
        }

        return result;
    }

    [Benchmark]
    public int SumLINQ() => numbers.Sum();

    [Benchmark]
    public int SumPLQING() => numbers.AsParallel().Sum();


    [Benchmark]
    public int SIMDVectors()
    {
        var vectorSize = Vector<int>.Count;
        var accVector = Vector<int>.Zero;
        for (var i = 0; i <= numbers.Length - vectorSize; i += vectorSize)
        {
            var v = new Vector<int>(numbers, i);
            accVector = Vector.Add(accVector, v);
        }

        var result = Vector.Dot(accVector, Vector<int>.One);
        for (var i = 0; i < numbers.Length; i++)
        {
            result += numbers[i];
        }
        return result;
    }
}