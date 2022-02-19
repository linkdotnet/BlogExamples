using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmark>();

public enum Color
{
    Red = 0,
    Green = 1,
}

public class Benchmark
{
    private readonly Color _colorRed = Color.Red;
    private readonly Color _colorGreen = Color.Green;

    [Benchmark(Baseline = true)]
    public bool ObjectEquals() => Equals(_colorRed, _colorGreen);

    [Benchmark]
    public bool EnumEquals() => Enum.Equals(_colorRed, _colorGreen);

    [Benchmark]
    public bool InstanceEquals() => _colorRed.Equals(_colorGreen);

    [Benchmark]
    public bool ComparisonOperator() => _colorRed == _colorGreen;
}