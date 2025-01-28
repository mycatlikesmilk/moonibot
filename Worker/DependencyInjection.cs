using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;
using Worker.Common;
using Worker.Jobs;
using Worker.Service;

namespace Worker;

public static class DependencyInjection
{
    public static IServiceCollection AddWorker(this IServiceCollection services)
    {
        services.AddSingleton<IJobFactory, SingletonJobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        
        // jobs
        services.AddSingleton<JokeJob>();
        
        // schedules
        services.AddSingleton(new JobSchedule(
            jobType: typeof(JokeJob),
            cronExpression: "0 0 0/2 * * ?"
        ));

        services.AddHostedService<QuartzHostedService>();
        
        return services;
    }
}