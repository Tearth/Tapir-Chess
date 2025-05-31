using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Core.Scheduler;
using Tapir.Core.Types;
using Tapir.Services.Players.Application.Tasks;
using Tapir.Services.Players.Domain.Players.Entities;

namespace Tapir.Services.Players.Application.Admin.Commands
{
    public class RebuildReadModelCommand
    {

    }

    public interface IRebuildReadModelCommandHandler : ICommandHandler<RebuildReadModelCommand, Unit>
    {

    }

    public class RebuildReadModelCommandHandler : IRebuildReadModelCommandHandler
    {
        private readonly ITaskScheduler _taskScheduler;

        public RebuildReadModelCommandHandler(ITaskScheduler taskScheduler)
        {
            _taskScheduler = taskScheduler;
        }

        public async Task<Unit> Process(RebuildReadModelCommand command, ClaimsPrincipal? user)
        {
            await _taskScheduler.Run(new ReadModelRebuildingTask());
            return Unit.Default;
        }
    }
}
