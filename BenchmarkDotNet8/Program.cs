﻿using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmarks>();

[SimpleJob(RuntimeMoniker.Net70, baseline: true)]
[SimpleJob(RuntimeMoniker.Net80)]
[HideColumns(Column.Job, Column.RatioSD)]
public class Benchmarks
{
    private readonly List<int> _numbers = Enumerable.Range(0, 10000).Select(s => Random.Shared.Next()).ToList();
    private readonly string _text1 = "Hello woRld! ThIs is a test.It CoUlD be a lot longer, but it'S not. Or is it?";
    private readonly string _text2 = "Hello world! This is a test.it could be a lot longer, but it's not. Or is it?";

    [Benchmark]
    public bool EqualsOrdinalIgnoreCase() => _text1.Equals(_text2, StringComparison.OrdinalIgnoreCase);

    [Benchmark]
    public bool EqualsOrdinal() => _text1.Equals(_text2, StringComparison.Ordinal);

    [Benchmark]
    public bool ContainsOrdinalIgnoreCase() => _text1.Contains("Or is it?", StringComparison.OrdinalIgnoreCase);

    [Benchmark]
    public bool ContainsOrdinal() => _text1.Contains("Or is it?", StringComparison.Ordinal);

    [Benchmark]
    public bool StartsWithOrdinalIgnoreCase() => _text1.StartsWith("Hello World", StringComparison.OrdinalIgnoreCase);

    [Benchmark]
    public bool StartsWithOrdinal() => _text1.StartsWith("Hello World", StringComparison.Ordinal);

    [Benchmark]
    public int LinqMin() => _numbers.Min();

    [Benchmark]
    public int LinqMax() => _numbers.Max();

    [Benchmark]
    public int ReflectionCreateInstance() => (int)Activator.CreateInstance(typeof(int));

    [Benchmark]
    public string InvokeMethodViaReflection()
    {
        var method = typeof(string).GetMethod("ToLowerInvariant", BindingFlags.Public | BindingFlags.Instance);
        return (string)method.Invoke(_text1, null);
    }

    [Benchmark]
    public DayOfWeek EnumParse() => Enum.Parse<DayOfWeek>("Saturday");

    [Benchmark]
    public DayOfWeek[] EnumGetValues() => Enum.GetValues<DayOfWeek>();

    [Benchmark]
    public string[] EnumGetNames() => Enum.GetNames<DayOfWeek>();
}