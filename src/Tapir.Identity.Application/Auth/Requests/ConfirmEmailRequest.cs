namespace Tapir.Identity.Application.Auth.Requests
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
