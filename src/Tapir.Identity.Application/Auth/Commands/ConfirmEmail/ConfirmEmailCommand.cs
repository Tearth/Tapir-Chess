using MediatR;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Validation;

namespace Tapir.Identity.Application.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ConfirmEmailCommandResult>
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string UserId { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Token { get; set; }
    }
}
