using Microsoft.EntityFrameworkCore;
using MindNote.Data.Raws;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<RawUser> Users { get; set; }

        public DbSet<RawNote> Notes { get; set; }

        public DbSet<RawCategory> Categories { get; set; }
    }
}
