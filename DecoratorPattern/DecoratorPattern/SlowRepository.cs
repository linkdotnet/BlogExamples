namespace DecoratorPattern;

public class SlowRepository : IRepository
{
    private readonly List<Person> _people = new();

    public async Task<Person> GetPersonById(int id)
    {
        await Task.Delay(1000);
        return _people.Single(p => p.Id == id);
    }

    public Task SavePersonAsync(Person person)
    {
        _people.Add(person);
        return Task.CompletedTask;
    }
}