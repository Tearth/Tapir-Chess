using System.Security.Claims;

namespace Tapir.Core.Identity
{
    public static class IdentityExtensions
    {
        private const string ID_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private const string EMAIL_CLAIM = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        private const string NAME_CLAIM = "name";

        public static Guid? GetId(this ClaimsPrincipal principal)
        {
            if (principal.Claims.FirstOrDefault(p => p.Type == ID_CLAIM)?.Value is not string id)
            {
                return null;
            }

            return Guid.Parse(id);
        }

        public static string? GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(p => p.Type == EMAIL_CLAIM)?.Value;
        }

        public static string? GetName(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(p => p.Type == NAME_CLAIM)?.Value;
        }
    }
}
