using System.Security.Cryptography.X509Certificates;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceNet10;

[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MarkdownExporterAttribute.GitHub]
[KeepBenchmarkFiles]
[MemoryDiagnoser]
public class LinqTests
{
    private static readonly IReadOnlyCollection<int> Numbers = Enumerable.Range(0, 20000).ToArray();
    private static readonly IReadOnlyCollection<float> NumbersFloats = Enumerable.Range(0, 20000).Select(s => (float)s).ToArray();
    
    [Benchmark]
    public int EvenCountInteger()
    {
        return Numbers.Count(n => n % 2 == 0);
    }
    
    [Benchmark]
    public IReadOnlyCollection<int> EvenCountIntegerToList()
    {
        return Numbers.Where(n => n % 2 == 0).ToList();
    }
    
    [Benchmark]
    public int EvenCountFloat()
    {
        return NumbersFloats.Count(n => n % 2 == 0);
    }
}