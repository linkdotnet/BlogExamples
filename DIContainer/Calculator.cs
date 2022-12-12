namespace DIContainer;

public interface ICalculator
{
    int Add(int a, int b);
}

public class Calculator : ICalculator
{
    public int Add(int a, int b) => a + b;
}