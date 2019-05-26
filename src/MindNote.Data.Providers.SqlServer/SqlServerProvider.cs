using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text;
using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    public class SqlServerProvider : IDataProvider
    {
        NodesProvider nodes;
        StructsProvider structs;
        TagsProvider tags;
        RelationsProvider relations;

        public SqlServerProvider(DataContext context)
        {
            nodes = new NodesProvider(context);
            structs = new StructsProvider(context);
            tags = new TagsProvider(context);
            relations = new RelationsProvider(context);
        }

        public INodesProvider GetNodesProvider() => nodes;

        public IRelationsProvider GetRelationsProvider() => relations;

        public IStructsProvider GetStructsProvider() => structs;

        public ITagsProvider GetTagsProvider() => tags;
    }

    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public const string LocalConnection = "Server=(localdb)\\mssqllocaldb;Database=mndb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public DataContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<DataContext> optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(LocalConnection);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
