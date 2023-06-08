public interface IMediator
{
    void Send<TMessage>(TMessage message);
}