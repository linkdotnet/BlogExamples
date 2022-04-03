// See https://aka.ms/new-console-template for more information

using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using LinkDotNet;

 BenchmarkRunner.Run<Benchi>();

[MemoryDiagnoser]
public class Benchi
{
    [Benchmark(Baseline = true)]
    public string DotNet()
    {
        var reference = new StringBuilder();
        return reference
            .AppendLine("Hello World")
            .AppendLine("Here some other text")
            .AppendLine("And again some other text as well for good measure")
            .AppendLine("You are still here?")
            .AppendLine("Hmmm.")
            .AppendLine("I wish you a very nice day and all the best.")
            .AppendLine("Sincerly Steven")
            .ToString();
    }

    [Benchmark]
    public string Fast()
    {
        var fast = new ValueStringBuilder();
        fast.AppendLine("Hello World");
        fast.AppendLine("Here some other text");
        fast.AppendLine("And again some other text as well for good measure");
        fast.AppendLine("You are still here?");
        fast.AppendLine("I wish you a very nice day and all the best.");
        fast.AppendLine("Sincerly Steven");
        return fast.ToString();
    }
}