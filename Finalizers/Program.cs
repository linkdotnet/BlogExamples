// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<EmptyFinalizer>();

[MemoryDiagnoser]
public class EmptyFinalizer
{
    [Benchmark(Baseline = true)]
    public ClassWithoutFinalizer WithoutFinalizer() => new(); 
    
    [Benchmark]
    public ClassWithEmptyFianlizer WithEmptyFinalizer() => new();
}

public class ClassWithEmptyFianlizer
{
    ~ClassWithEmptyFianlizer() { }
}

public class ClassWithoutFinalizer
{
}