using Microsoft.AspNetCore.Identity;

namespace Tapir.Identity.Infrastructure.Models
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {

        }

        public ApplicationRole(string roleName) : base(roleName)
        {

        }
    }
}
