namespace Tapir.Identity.Application.Auth.Commands.Login
{
    public class LogInCommandResponse
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
