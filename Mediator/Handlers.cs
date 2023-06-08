public class EmailSender : INotificationHandler<EmailMessage>
{
    public void Handle(EmailMessage notification)
    {
        Console.WriteLine($"Inside {nameof(EmailSender)}.Handle()...");
        Console.WriteLine($"Sending email with subject {notification.Subject}...");
    }
}

public class EmailArchiver : INotificationHandler<EmailMessage>
{
    public void Handle(EmailMessage notification)
    {
        Console.WriteLine($"Inside {nameof(EmailArchiver)}.Handle()...");
        Console.WriteLine($"Archiving email with subject {notification.Subject}...");
    }
}