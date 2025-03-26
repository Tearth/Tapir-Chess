using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Tapir.Identity.Application.Account.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ChangePasswordCommandResponse>
    {
        [Required]
        public required string OldPassword { get; set; }

        [Required]
        public required string NewPassword { get; set; }
    }
}
