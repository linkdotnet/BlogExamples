namespace DIContainer;

public interface IMultiplier
{
    int Multiply(int a, int b);
}

public class Multiplier : IMultiplier
{
    private readonly ICalculator _calculator;

    public Multiplier(ICalculator calculator)
    {
        _calculator = calculator;
    }

    public int Multiply(int a, int b)
    {
        if (a == 0 || b == 0)
            return 0;

        var result = 0;
        for (var i = 0; i < Math.Abs(b); i++)
            result += a;

        return Math.Sign(b) == 1 ? result : -result;
    }
}