namespace Tapir.Identity.Application.Auth.Responses
{
    public class RefreshTokenResponse
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
