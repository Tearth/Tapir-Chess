using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Infrastructure.Persistence
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
    }
}
