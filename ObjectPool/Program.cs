using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<PoolBenchmark>();

[MemoryDiagnoser]
public class PoolBenchmark
{
    private const int LoopCount = 10;

    [Benchmark(Baseline = true)]
    public int CreateNewObjects()
    {
        var sum = 0;
        for (var i = 0; i < LoopCount; i++)
        {
            var obj = new MyHeavyClass();
            sum += obj.Length;
        }
        
        return sum;
    }

    [Benchmark]
    public int ObjectPool()
    {
        var  pool = new ObjectPool<MyHeavyClass>(() => new MyHeavyClass());
        var sum = 0;
        for (var i = 0; i < LoopCount; i++)
        {
            var obj = pool.Rent();
            sum += obj.Length;
            pool.Return(obj);
        }
        
        return sum;
    }
}

public class MyHeavyClass
{
    private readonly double[] _someArray;

    public MyHeavyClass()
    {
        _someArray = new double[5000];
        Array.Fill(_someArray, 10);
    }

    public int Length => _someArray.Length;
}