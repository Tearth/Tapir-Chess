namespace Tapir.Identity.Application.Auth.Responses
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public List<String> Errors { get; set; }
    }
}
