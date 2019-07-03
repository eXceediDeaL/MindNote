using Microsoft.EntityFrameworkCore;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Relation> Relations { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
