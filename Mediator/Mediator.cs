public class Mediator : IMediator
{
    private readonly NotificationHandlerRegistry handlerRegistry;

    public Mediator(NotificationHandlerRegistry handlerRegistry)
    {
        this.handlerRegistry = handlerRegistry;
    }

    public void Send<TMessage>(TMessage message)
    {
        if (!handlerRegistry.HasHandler<TMessage>())
        {
            throw new InvalidOperationException($"No handler registered for message type {typeof(TMessage).Name}");
        }
        var handlers = handlerRegistry.GetHandlers<TMessage>();
        foreach (var handler in handlers)
        {
            handler.Handle(message);
        }
    }
}