namespace CronBackgroundWorker.Cron;

public sealed class CronScheduler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IReadOnlyCollection<CronRegistryEntry> _cronJobs;

    public CronScheduler(
        IServiceProvider serviceProvider,
        IEnumerable<CronRegistryEntry> cronJobs)
    {
        // Use the container
        _serviceProvider = serviceProvider;
        _cronJobs = cronJobs.ToList();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Create a timer that has a resolution less than 60 seconds
        // Because cron has a resolution of a minute
        // So everything under will work
        using var tickTimer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        // Create a map of the next upcoming entries
        var runMap = new Dictionary<DateTime, List<Type>>();
        while (await tickTimer.WaitForNextTickAsync(stoppingToken))
        {
            // Get UTC Now with minute resolution (remove microseconds and seconds)
            var now = UtcNowMinutePrecision();

            // Run jobs that are in the map
            RunActiveJobs(runMap, now, stoppingToken);

            // Get the next run for the upcoming tick
            runMap = GetJobRuns();
        }
    }

    private void RunActiveJobs(IReadOnlyDictionary<DateTime, List<Type>> runMap, DateTime now, CancellationToken stoppingToken)
    {
        if (!runMap.TryGetValue(now, out var currentRuns))
        {
            return;
        }

        foreach (var run in currentRuns)
        {
            // We are sure (thanks to our extension method)
            // that the service is of type ICronJob
            var job = (ICronJob)_serviceProvider.GetRequiredService(run);

            // We don't want to await jobs explicitly because that
            // could interfere with other job runs
            job.Run(stoppingToken);
        }
    }

    private Dictionary<DateTime, List<Type>> GetJobRuns()
    {
        var runMap = new Dictionary<DateTime, List<Type>>();
        foreach (var cron in _cronJobs)
        {
            var utcNow = DateTime.UtcNow;
            var runDates = cron.CrontabSchedule.GetNextOccurrences(utcNow, utcNow.AddMinutes(1));
            if (runDates is not null)
            {
                AddJobRuns(runMap, runDates, cron);
            }
        }

        return runMap;
    }

    private static void AddJobRuns(IDictionary<DateTime, List<Type>> runMap, IEnumerable<DateTime> runDates, CronRegistryEntry cron)
    {
        foreach (var runDate in runDates)
        {
            if (runMap.TryGetValue(runDate, out var value))
            {
                value.Add(cron.Type);
            }
            else
            {
                runMap[runDate] = new List<Type> { cron.Type };
            }
        }
    }

    private static DateTime UtcNowMinutePrecision()
    {
        var now = DateTime.UtcNow;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
    }
}