namespace Tapir.Identity.Application.Auth.Requests
{
    public class ConfirmPasswordRequest
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
