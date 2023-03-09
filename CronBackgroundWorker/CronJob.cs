using CronBackgroundWorker.Cron;

namespace CronBackgroundWorker;

public class CronJob : ICronJob
{
    private readonly ILogger<CronJob> _logger;

    public CronJob(ILogger<CronJob> logger)
    {
        _logger = logger;
    }
    public Task Run(CancellationToken token = default)
    {
        _logger.LogInformation("Hello from {name} at: {time}", nameof(CronJob), DateTime.UtcNow.ToShortTimeString());

        return Task.CompletedTask;
    }
}