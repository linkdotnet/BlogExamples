// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmarks>();

[RankColumn]
public class Benchmarks
{
    private static readonly int[] Values = Enumerable.Range(0, 50_000).ToArray();

    [Benchmark(Baseline = true)]
    public int SumForLoop() 
    {
        var acc = 0;
        for (var i = 0; i < Values.Length; i++)
            acc += i;

        return acc;
    }
    
    [Benchmark]
    public int SumLinq() => Values.Sum();
    
    [Benchmark]
    public int SumPLinq() => Values.AsParallel().Sum();

    [Benchmark]
    public int SumLinqSimdNaive()
    {
        // The performant way of getting an array of vectors rather than doing it by hand
        var vectors = MemoryMarshal.Cast<int, Vector<int>>(Values);
        var accVector = Vector<int>.Zero;
    
        foreach (var vector in vectors)
        {
            accVector += vector;
        }

        var remainder = 0;
        for (var i = Values.Length - (Values.Length % Vector<int>.Count); i < Values.Length; i++)
        {
            remainder += Values[i];
        }
    
        // Combine vector sum and remainder
        return Vector.Sum(accVector) + remainder;
    }

    [Benchmark]
    public int SumLinqSimdBetter()
    {
        /*
         * In comparison to the naive version, this one is a bit more complex.
         *
         * Instead of only using "one" Vector<int> to sum the elements,
         * we use two vectors at a time and put the result into the acc vector.
         * This enables ILP (Instruction Level Parallelism) and
         * allows the CPU to execute multiple instructions
         */
        var spanAsVectors = MemoryMarshal.Cast<int, Vector<int>>(Values);
        var remainingElements = Values.Length % Vector<int>.Count;
        var accVector = Vector<int>.Zero;

        for (var i = 0; i < spanAsVectors.Length - 1; i += 2)
        {
            accVector += spanAsVectors[i] + spanAsVectors[i + 1];
        }

        if (spanAsVectors.Length % 2 == 1)
        {
            accVector += spanAsVectors[^1];
        }

        if (remainingElements > 0)
        {
            var startingLastElements = Values.Length - remainingElements;
            var remainingElementsSlice = Values[startingLastElements..];
        
            accVector += new Vector<int>(remainingElementsSlice);
        }

        return Vector.Sum(accVector);
    }

    [Benchmark]
    public int SumLinqSimdUnrolled4()
    {
        var vectors = MemoryMarshal.Cast<int, Vector<int>>(Values);
        var simdCount = vectors.Length;

        // Four accumulators to enable EVEN MORE ILP
        var acc1 = Vector<int>.Zero;
        var acc2 = Vector<int>.Zero;
        var acc3 = Vector<int>.Zero;
        var acc4 = Vector<int>.Zero;

        var i = 0;
        for (; i <= simdCount - 4; i += 4)
        {
            acc1 += vectors[i];
            acc2 += vectors[i + 1];
            acc3 += vectors[i + 2];
            acc4 += vectors[i + 3];
        }

        // Combine the accumulators
        var accVector = acc1 + acc2 + acc3 + acc4;

        // Handle remaining vectors if length % 4 != 0
        for (; i < simdCount; i++)
        {
            accVector += vectors[i];
        }

        // Handle remaining elements that didn't fit into a vector
        var remainingElements = Values.Length % Vector<int>.Count;
        if (remainingElements > 0)
        {
            Span<int> lastVectorElements = stackalloc int[Vector<int>.Count];
            Values[^remainingElements..].CopyTo(lastVectorElements);
            accVector += new Vector<int>(lastVectorElements);
        }

        return Vector.Sum(accVector);
    }
}