using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Tapir.Identity.Application.Auth.Commands.ConfirmPassword
{
    public class ConfirmPasswordCommand : IRequest<bool>
    {
        [Required]
        public required string UserId { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
