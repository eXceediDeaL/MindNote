using Microsoft.EntityFrameworkCore;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class IdentityDataContext : DbContext
    {
        public IdentityDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
    }
}
