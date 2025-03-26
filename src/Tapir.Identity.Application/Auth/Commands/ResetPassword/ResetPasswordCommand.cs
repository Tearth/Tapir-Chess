using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
