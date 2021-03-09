using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LegiosoftTestApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public virtual new DbSet<IdentityUser> Users { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {            
        }

    }
}
