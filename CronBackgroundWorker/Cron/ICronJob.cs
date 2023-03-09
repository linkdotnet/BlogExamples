namespace CronBackgroundWorker.Cron;

public interface ICronJob
{
    Task Run(CancellationToken token = default);
}