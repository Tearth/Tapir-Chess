using System.Security.Claims;

namespace Tapir.Core.Commands
{
    public interface ICommandHandler<TCommand, TResult>
    {
        Task<TResult> Process(TCommand command, ClaimsPrincipal? user);
    }
}
