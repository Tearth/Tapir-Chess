using MediatR;
using System.ComponentModel.DataAnnotations;
using Tapir.Identity.Application.Auth.Commands.Register;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class RegisterCommand : IRequest<RegisterCommandResponse>
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
