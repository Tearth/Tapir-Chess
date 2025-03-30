using Tapir.Identity.Application.Auth.Commands.ConfirmPassword;
using Tapir.Identity.Infrastructure.Commands;

namespace Tapir.Identity.Application.Auth.Commands.Login
{
    public class LogInCommandResult : CommandResultBase<LogInCommandResult>
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
