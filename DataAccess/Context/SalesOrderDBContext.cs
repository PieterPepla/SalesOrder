using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace DataAccess.Context
{
    public class SalesOrderDBContext : IdentityDbContext<User>
    {
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderLine> OrderLine { get; set; }

        public SalesOrderDBContext(DbContextOptions<SalesOrderDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
