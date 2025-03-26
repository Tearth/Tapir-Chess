using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Tapir.Identity.Application.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<bool>
    {
        [Required]
        public required string UserId { get; set; }

        [Required]
        public required string Token { get; set; }
    }
}
