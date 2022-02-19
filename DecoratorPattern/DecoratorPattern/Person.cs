namespace DecoratorPattern;

public class Person
{
    public Person(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }
}