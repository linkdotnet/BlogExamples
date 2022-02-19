namespace DecoratorPattern;

public interface IRepository
{
    public Task<Person> GetPersonByIdAsync(int id);

    public Task SavePersonAsync(Person person);
}