public class NotificationHandlerRegistry
{
    // The handler has to hold objects as key, as we don't know the type of the message yet
    private readonly Dictionary<Type, object> handlers = new();

    public void AddHandler<T>(INotificationHandler<T> handler)
    {
        var messageType = typeof(T);

        if (!handlers.ContainsKey(messageType))
        {
            handlers[messageType] = new List<INotificationHandler<T>>();
        }

        ((List<INotificationHandler<T>>)handlers[messageType]).Add(handler);
    }

    public bool HasHandler<T>() => handlers.ContainsKey(typeof(T));

    public IReadOnlyCollection<INotificationHandler<T>> GetHandlers<T>()
        => handlers.TryGetValue(typeof(T), out var list) ? (List<INotificationHandler<T>>)list : Array.Empty<INotificationHandler<T>>();
}