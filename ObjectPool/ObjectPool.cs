using System.Collections.Concurrent;

public class ObjectPool<T>
{
    private readonly ConcurrentBag<T> _objects;
    private readonly Func<T> _objectGenerator;

    /// <summary>
    /// Initializes the ObjectPool.
    /// </summary>
    /// <param name="objectGenerator">We need a generator function to create an object if our pool is empty.</param>
    public ObjectPool(Func<T> objectGenerator)
    {
        _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
        _objects = new ConcurrentBag<T>();
    }
    
    public T Rent() => _objects.TryTake(out T item) ? item : _objectGenerator();

    public void Return(T item) => _objects.Add(item);
}