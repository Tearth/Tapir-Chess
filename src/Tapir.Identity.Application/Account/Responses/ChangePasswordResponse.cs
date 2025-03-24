namespace Tapir.Identity.Application.Account.Responses
{
    public class ChangePasswordResponse
    {
        public bool Success { get; set; }
        public List<String> Errors { get; set; }
    }
}
