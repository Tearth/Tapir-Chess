namespace Tapir.Identity.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandResponse
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
