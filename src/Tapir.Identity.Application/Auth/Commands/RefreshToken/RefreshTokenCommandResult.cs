using Tapir.Identity.Application.Auth.Commands.Login;
using Tapir.Identity.Infrastructure.Commands;

namespace Tapir.Identity.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandResult : CommandResultBase<RefreshTokenCommandResult>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
