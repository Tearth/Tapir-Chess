namespace Tapir.Identity.Application.Auth.Commands.Register
{
    public class RegisterCommandResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
