using CronBackgroundWorker.Cron;

namespace CronBackgroundWorker;

public class AnotherCronJob : ICronJob
{
    public Task Run(CancellationToken token = default)
    {
        Console.WriteLine($"Hello from {nameof(AnotherCronJob)} at: {DateTime.UtcNow.ToShortTimeString()}");

        return Task.CompletedTask;
    }
}