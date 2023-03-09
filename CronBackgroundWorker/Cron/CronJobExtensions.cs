using Microsoft.Extensions.DependencyInjection.Extensions;
using NCrontab;

namespace CronBackgroundWorker.Cron;

public static class CronJobExtensions
{
    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, string cronExpression)
        where T : class, ICronJob
    {
        var cron = CrontabSchedule.TryParse(cronExpression)
                   ?? throw new ArgumentException("Invalid cron expression", nameof(cronExpression));

        var entry = new CronRegistryEntry(typeof(T), cron);

        // AddHostedService internally only registers one time
        services.AddHostedService<CronScheduler>();

        // TryAdd prevents multiple registrations of T
        services.TryAddSingleton<T>();
        services.AddSingleton(entry);

        return services;
    }
}