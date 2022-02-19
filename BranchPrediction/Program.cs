
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchi>();

[HardwareCounters(HardwareCounter.BranchMispredictions)]
public class Benchi
{
    private const int ArraySize = 65535;
    private int[] unsortedArray = new int[ArraySize];
    private int[] sortedArray = new int[ArraySize];

    public Benchi()
    {
        var random = new Random();
        for (var i = 0; i < ArraySize; i++)
        {
            unsortedArray[i] = sortedArray[i] = random.Next(256);
        }

        Array.Sort(sortedArray);
    }

    [Benchmark(Baseline = true)]
    public int Unsorted() => SumOfEverythingAbove128Unsorted(unsortedArray);

    [Benchmark]
    public int Sorted() => SumOfEverythingAbove128Unsorted(sortedArray);

    private static int SumOfEverythingAbove128Unsorted(int[] data)
    {
        var sum = 0;
        for (var i = 0; i < data.Length; i++)
        {
            if (data[i] >= 128)
            {
                sum += data[i];
            }
        }

        return sum;
    }
}