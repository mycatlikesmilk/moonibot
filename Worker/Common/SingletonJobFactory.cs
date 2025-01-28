using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Worker.Common;

public class SingletonJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SingletonJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        // Создаем задачу через DI-контейнер
        return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job)
    {
        // Освобождаем ресурсы, если нужно
        if (job is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}