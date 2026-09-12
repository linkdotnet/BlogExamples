using DuckType;

namespace DuckType.Sample;

[DuckShape]
public interface IDoable
{
    void Do();
}

[DuckShape]
public interface INameable
{
    string Name { get; set; }
}

public class A
{
    public void Do() => Console.WriteLine("A.Do");
}

public class B
{
    public void Do() => Console.WriteLine("B.Do");
}

public class Person
{
    public string Name { get; set; } = "";
}

public static partial class Ops
{
    [DuckTyped]
    public static void Foo(IDoable a) => a.Do();

    [DuckTyped]
    public static void Greet(INameable n) => Console.WriteLine($"Hello, {n.Name}!");
}

public static class Program
{
    public static void Main()
    {
        Ops.Foo(new A());
        Ops.Foo(new B());

        var person = new Person { Name = "Steven" };
        Ops.Greet(person);
    }
}
