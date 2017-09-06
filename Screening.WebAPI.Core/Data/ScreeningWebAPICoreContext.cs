using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Screening.WebAPI.Core.Data
{
    public class ScreeningWebApiCoreContext : IdentityDbContext<User, Role, string>
    {
        public ScreeningWebApiCoreContext(DbContextOptions<ScreeningWebApiCoreContext> options)
            : base(options)
        {
        }

        public DbSet<Mark> Marks { get; set; }
    }
}
