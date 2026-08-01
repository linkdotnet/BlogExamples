using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[SimpleJob(RuntimeMoniker.Net11_0)]
public class EnumBenchmarks
{
    private DayOfWeek _left = DayOfWeek.Wednesday;
    private DayOfWeek _right = DayOfWeek.Friday;

    [Benchmark]
    public bool GenericEquals() => AreEqual(_left, _right);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool AreEqual<T>(T left, T right)
        where T : struct, Enum
        => left.Equals(right);
}