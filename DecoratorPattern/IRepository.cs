namespace DecoratorPattern;

public interface IRepository
{
    public Task<Person> GetPersonById(int id);

    public Task SavePersonAsync(Person person);
}