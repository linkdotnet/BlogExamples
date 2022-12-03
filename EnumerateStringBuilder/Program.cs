using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmark>();

[MemoryDiagnoser]
public class Benchmark
{
    private StringBuilder _sb = default!;

    [Params(10, 100, 1000)]
    public int Chars { get; set; }

    [GlobalSetup]
    public void Setup() => _sb = new StringBuilder(new string('c', Chars));

    [Benchmark(Baseline = true)]
    public void EnumeratorViaToString()
    {
        foreach (var c in _sb.ToString()) _ = c;
    }

    [Benchmark]
    public void EnumeratorViaExtension()
    {
        foreach (var c in _sb) _ = c;
    }

    [Benchmark]
    public void IterateViaForLoop()
    {
        for (var i = 0; i < _sb.Length; i++)
            _ = _sb[i];
    }

    [Benchmark]
    public void ViaChunkEnumerator()
    {
        foreach (var chunk in _sb.GetChunks())
        {
            foreach (var c in chunk.Span)
                _ = c;
        }
    }
}

public static class Extensions
{
    public static StringEnumerator GetEnumerator(this StringBuilder s) => new(s);
}

/// <summary>
/// ref structs can't escape to the heap
/// We do this to further optimize the runtime and allocations
/// I know that mutable value types are evil, but normally this type
/// is not handled by user-code, so it is fine
/// </summary>
public ref struct StringEnumerator
{
    private readonly StringBuilder _stringBuilder;
    private int _index = -1;
    public StringEnumerator(StringBuilder stringBuilder) => _stringBuilder = stringBuilder;

    public bool MoveNext()
    {
        _index++;
        return _index < _stringBuilder.Length;
    }

    /// <summary>
    /// This is potentially dangerous as this is not always O(1)
    /// The StringBuilder has chunks, which are at most 8000 bytes long
    /// So having a string longer than 8000 bytes might have to access
    /// the second chunk.
    /// </summary>
    public char Current => _stringBuilder[_index];

    public void Dispose() => _index = -1;
}