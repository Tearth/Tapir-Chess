using Microsoft.AspNetCore.Identity;

namespace Tapir.Identity.Infrastructure.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() : base()
        {

        }

        public ApplicationUser(string userName) : base(userName)
        {

        }
    }
}
