﻿using Newtonsoft.Json;
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

            if (string.IsNullOrEmpty(type))
            {
                throw new InvalidOperationException("Task assembly type is not set.");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new InvalidOperationException("Task data is not set.");
            }

            var assemblyType = Type.GetType(type);
            if (assemblyType == null)
            {
                throw new InvalidOperationException("Job assembly type not found.");
            }

            if (_serviceProvider.GetService(assemblyType) is not ITask task)
            {
                throw new InvalidOperationException("Job assembly type is not a valid task.");
            }

            JsonConvert.PopulateObject(data, task);

            await task.Run();
        }
    }
}
