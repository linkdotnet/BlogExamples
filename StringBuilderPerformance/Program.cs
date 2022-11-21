using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<StringBuilderBenchmark>();

[MemoryDiagnoser]
public class StringBuilderBenchmark
{
    private const string TextToAppend = "Hello World from Steven";

    [Params(5, 10, 25)]
    public int Iterations { get; set; }

    [Benchmark]
    public string StringBuilder()
    {
        var builder = new StringBuilder();
        for (var i = 0; i < Iterations; i++)
            builder.Append(TextToAppend);

        return builder.ToString();
    }

    [Benchmark]
    public string ListOfStrings()
    {
        var list = new List<string>();
        for(var i = 0; i< Iterations;i++)
            list.Add(TextToAppend);

        return string.Join(string.Empty, list);
    }

    [Benchmark]
    public string Concat()
    {
        var output = string.Empty;
        for (var i = 0; i < Iterations; i++)
            output += TextToAppend;

        return output;
    }
}