using Newtonsoft.Json;
using Quartz;
using Tapir.Core.Scheduler;

namespace Tapir.Providers.Scheduler.Quartz.Tasks
{
    public class TaskWrapper : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public TaskWrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var type = context.JobDetail.JobDataMap.GetString("Type");
            var data = context.JobDetail.JobDataMap.GetString("Data");

            var assemblyType = Type.GetType(type);
            if (assemblyType == null)
            {
                // TODO: log this
                return;
            }

            var task = _serviceProvider.GetService(assemblyType) as ITask;
            if (task == null)
            {
                // TODO: Log this
                return;
            }

            JsonConvert.PopulateObject(data, task);

            await task.Run();
        }
    }
}
