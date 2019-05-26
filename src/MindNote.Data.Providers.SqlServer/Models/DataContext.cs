using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }

        public DbSet<Struct> Structs { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Relation> Relations { get; set; }
    }
}
