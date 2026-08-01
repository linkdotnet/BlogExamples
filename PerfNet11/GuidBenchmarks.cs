using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[SimpleJob(RuntimeMoniker.Net11_0)]
public class GuidBenchmarks
{
    private const string DFormat =
        "00112233-4455-6677-8899-aabbccddeeff";

    private const string NFormat =
        "00112233445566778899aabbccddeeff";

    [Benchmark]
    public Guid ParseD()
        => Guid.Parse(DFormat);

    [Benchmark]
    public Guid ParseN()
        => Guid.Parse(NFormat);

    [Benchmark]
    public Guid ParseExactD()
        => Guid.ParseExact(DFormat, "D");

    [Benchmark]
    public bool TryParseD()
        => Guid.TryParse(DFormat, out _);

    [Benchmark]
    public Guid Constructor()
        => new(DFormat);
}