using NCrontab;

namespace CronBackgroundWorker.Cron;

public sealed record CronRegistryEntry(Type Type, CrontabSchedule CrontabSchedule);