using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace TestApp.Models
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public virtual new DbSet<IdentityUser> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
