using System.Runtime.CompilerServices;

foreach (var i in 12)
{
    Console.WriteLine(i);
}

await new TimeSpan(0, 0, 2);
await 2.Seconds();

// Real shenanigans start here
public static class Extensions
{
    public static IntEnumerator GetEnumerator(this int i) => new(i);
    public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
    {
        Console.WriteLine($"Waiting {timeSpan}");
        return Task.Delay(timeSpan).GetAwaiter();
    }

    public static TimeSpan Seconds(this int i) => new(0, 0, i);
}

public struct IntEnumerator
{
    private readonly List<int> _listOfInts;
    private int _index = -1;

    public IntEnumerator(int num)
    {
        _listOfInts = new List<int>();
        while(num > 0)
        {
            _listOfInts.Add(num % 10);
            num /= 10;
        }
        _listOfInts.Reverse();
    }

    public int Current => _listOfInts[_index];

    public bool MoveNext()
    {
        _index++;
        return _listOfInts.Count > _index;
    }
}