namespace Tapir.Identity.Infrastructure
{
    public class AppSettings
    {
        public JWTSettings JWT { get; set; }
    }

    public class JWTSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
    }
}
