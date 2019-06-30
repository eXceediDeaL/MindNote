using System;
using System.Text;

namespace MindNote.Data.Providers.InMemory
{
    public class DataProvider : IDataProvider
    {
        public DataProvider()
        {
            NodesProvider = new NodesProvider(this);
            RelationsProvider = new RelationsProvider(this);
            TagsProvider = new TagsProvider(this);
        }

        public INodesProvider NodesProvider { get; private set; }

        public IRelationsProvider RelationsProvider { get; private set; }

        public ITagsProvider TagsProvider { get; private set; }
    }

    struct Model<T>
    {
        public T Data { get; set; }

        public string UserId { get; set; }
    }
}
