using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Tapir.Identity.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenCommandResponse>
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
