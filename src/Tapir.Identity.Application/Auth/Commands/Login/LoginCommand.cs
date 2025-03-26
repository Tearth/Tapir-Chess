using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Tapir.Identity.Application.Auth.Commands.Login
{
    public class LogInCommand : IRequest<LogInCommandResponse>
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
