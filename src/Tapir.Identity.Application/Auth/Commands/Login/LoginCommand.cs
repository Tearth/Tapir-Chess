using MediatR;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Validation;

namespace Tapir.Identity.Application.Auth.Commands.Login
{
    public class LogInCommand : IRequest<LogInCommandResult>
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Username { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Password { get; set; }
    }
}
