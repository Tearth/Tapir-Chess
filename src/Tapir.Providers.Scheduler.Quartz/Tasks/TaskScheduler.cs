using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;
using System.Threading.Tasks;
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
                .UsingJobData("Data", JsonSerializer.Serialize(task))
                .PersistJobDataAfterExecution()
                .StoreDurably()
                .RequestRecovery()
                .Build();
            var trigger = TriggerBuilder.Create().StartNow().Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task Register<TTask>(TTask task, string cron) where TTask : ITask
        {
            var key = new JobKey(task.GetType().Name, "SCHEDULED");
            var trigger = TriggerBuilder.Create()
                .WithIdentity(task.GetType().Name)
                .WithCronSchedule(cron)
                .StartNow()
                .Build();

            _logger.LogInformation($"Registering task {key} with cron '{cron}'");

            await RegisterInternal(task, trigger, key);
        }


        public async Task Register<TTask>(TTask task, TimeSpan interval) where TTask : ITask
        {
            var key = new JobKey(task.GetType().Name, "SCHEDULED");
            var trigger = TriggerBuilder.Create()
                .WithIdentity(task.GetType().Name)
                .WithSchedule(SimpleScheduleBuilder.Create().WithInterval(interval).RepeatForever())
                .StartNow()
                .Build();

            _logger.LogInformation($"Registering task {key} with interval '{interval}'");

            await RegisterInternal(task, trigger, key);
        }

        private async Task RegisterInternal<TTask>(TTask task, ITrigger trigger, JobKey key) where TTask : ITask
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            if (await scheduler.CheckExists(key))
            {
                await scheduler.DeleteJob(key);
            }

            var job = JobBuilder.Create<TaskWrapper>()
                .WithIdentity(key)
                .UsingJobData("Type", task.GetType().AssemblyQualifiedName)
                .UsingJobData("Data", JsonSerializer.Serialize(task))
                .DisallowConcurrentExecution()
                .PersistJobDataAfterExecution()
                .StoreDurably()
                .Build();

            await scheduler.ScheduleJob(job, [trigger], true);
        }
    }
}
