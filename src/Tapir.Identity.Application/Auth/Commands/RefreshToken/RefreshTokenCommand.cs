using MediatR;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Validation;

namespace Tapir.Identity.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenCommandResult>
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string RefreshToken { get; set; }
    }
}
