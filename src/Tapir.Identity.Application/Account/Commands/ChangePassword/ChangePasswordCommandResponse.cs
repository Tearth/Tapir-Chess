namespace Tapir.Identity.Application.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
