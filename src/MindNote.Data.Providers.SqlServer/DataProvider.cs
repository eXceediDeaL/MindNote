using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    public class DataProvider : IDataProvider
    {
        public DataProvider(DataContext context)
        {
            NodesProvider = new NodesProvider(context, this);
            RelationsProvider = new RelationsProvider(context, this);
            TagsProvider = new TagsProvider(context, this);
        }

        public INodesProvider NodesProvider { get; }

        public IRelationsProvider RelationsProvider { get; }

        public ITagsProvider TagsProvider { get; }
    }
}
