using MediatR;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Validation;

namespace Tapir.Identity.Application.Account.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ChangePasswordCommandResult>
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string NewPassword { get; set; }
    }
}
