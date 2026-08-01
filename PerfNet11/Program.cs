using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<LinqMinMaxBenchmarks>();

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[SimpleJob(RuntimeMoniker.Net11_0)]
public class LinqMinMaxBenchmarks
{
    private byte[] _bytes = null!;
    private short[] _shorts = null!;
    private int[] _ints = null!;
    private long[] _longs = null!;

    [Params(16, 64, 1_024)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);

        _bytes = new byte[Length];
        _shorts = new short[Length];
        _ints = new int[Length];
        _longs = new long[Length];

        random.NextBytes(_bytes);

        for (var i = 0; i < Length; i++)
        {
            _shorts[i] = (short)random.Next(
                short.MinValue,
                short.MaxValue);

            _ints[i] = random.Next();

            _longs[i] = random.NextInt64();
        }
    }

    [Benchmark]
    public byte MinByte()
        => _bytes.Min();

    [Benchmark]
    public byte MaxByte()
        => _bytes.Max();

    [Benchmark]
    public short MinShort()
        => _shorts.Min();

    [Benchmark]
    public short MaxShort()
        => _shorts.Max();

    [Benchmark]
    public int MinInt()
        => _ints.Min();

    [Benchmark]
    public int MaxInt()
        => _ints.Max();

    [Benchmark]
    public long MinLong()
        => _longs.Min();

    [Benchmark]
    public long MaxLong()
        => _longs.Max();
}