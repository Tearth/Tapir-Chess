using MediatR;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Validation;
using Tapir.Identity.Application.Auth.Commands.ConfirmEmail;

namespace Tapir.Identity.Application.Auth.Commands.ConfirmPassword
{
    public class ConfirmPasswordCommand : IRequest<ConfirmPasswordCommandResult>
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string UserId { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Token { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Password { get; set; }
    }
}
