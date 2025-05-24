namespace Tapir.Core.Scheduler
{
    public interface ITaskScheduler
    {
        Task Run<TTask>(TTask task) where TTask : ITask;
        Task Register<TTask>(TTask task, string cron) where TTask : ITask;
        Task Register<TTask>(TTask task, TimeSpan interval) where TTask : ITask;
    }
}
