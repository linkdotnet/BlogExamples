public interface INotificationHandler<T>
{
    void Handle(T notification);
}
