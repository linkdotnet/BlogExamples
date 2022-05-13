using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Fibonacci>();

public class Fibonacci
{
    public const int FibonacciOf = 25;

    [Benchmark(Baseline = true)]
    public int FibonacciIterativeCall() => FibonacciIterative(FibonacciOf);

    [Benchmark]
    public int FibonacciRecursiveCall() => FibonacciRecursive(FibonacciOf);

    [Benchmark]
    public int FibonacciTailRecursiveCall() => FibonacciTailRecursive(FibonacciOf);
    
    private static int FibonacciIterative(int n)
    {
        if (n <= 1) return n;

        var (previous, current) = (0, 1);
        for (var i = 2; i < n; i++)
        {
            (previous, current) = (current, current + previous);
        }

        return current;
    }

    private static int FibonacciRecursive(int n)
    {
        if (n <= 1) return n;

        return FibonacciRecursive(n - 2) + FibonacciRecursive(n - 1);
    }

    private static int FibonacciTailRecursive(int n, int previous = 0, int current = 1)
    {
        if (n == 0)
            return previous;
        if (n == 1)
            return current;
        
        return FibonacciTailRecursive(n - 1, current, previous + current);
    }
}