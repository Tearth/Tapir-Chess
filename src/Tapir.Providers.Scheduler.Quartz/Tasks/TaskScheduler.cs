using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Tapir.Core.Scheduler;

namespace Tapir.Providers.Scheduler.Quartz.Tasks
{
    public class TaskScheduler : ITaskScheduler
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger<TaskScheduler> _logger;

        public TaskScheduler(ISchedulerFactory schedulerFactory, ILogger<TaskScheduler> logger)
        {
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }

        public async Task Run<TTask>(TTask task) where TTask : ITask
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var key = new JobKey($"{task.GetType().Name}_{Guid.NewGuid()}", "INSTANT");

            var job = JobBuilder.Create<TaskWrapper>()
                .WithIdentity(key)
                .UsingJobData("Type", task.GetType().AssemblyQualifiedName)
                .UsingJobData("Data", JsonConvert.SerializeObject(task))
                .PersistJobDataAfterExecution()
                .StoreDurably()
                .RequestRecovery()
                .Build();
            var trigger = TriggerBuilder.Create().StartNow().Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task Register<TTask>(TTask task, string cron) where TTask : ITask
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var key = new JobKey(task.GetType().Name, "SCHEDULED");

            _logger.LogInformation($"Registering task {key} with cron '{cron}'");

            var job = JobBuilder.Create<TaskWrapper>()
                .WithIdentity(key)
                .UsingJobData("Type", task.GetType().AssemblyQualifiedName)
                .UsingJobData("Data", JsonConvert.SerializeObject(task))
                .DisallowConcurrentExecution()
                .PersistJobDataAfterExecution()
                .StoreDurably()
                .Build();
            var trigger = TriggerBuilder.Create().WithCronSchedule(cron).StartNow().Build();

            await scheduler.ScheduleJob(job, [trigger], true);
        }
    }
}
