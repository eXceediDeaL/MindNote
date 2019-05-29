using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text;
using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    public class SqlServerProvider : IDataProvider
    {
        NodesProvider nodes;
        RelationsProvider relations;

        public SqlServerProvider(DataContext context)
        {
            nodes = new NodesProvider(context, this);
            relations = new RelationsProvider(context, this);
        }

        public INodesProvider NodesProvider => nodes;

        public IRelationsProvider RelationsProvider => relations;
    }
}
