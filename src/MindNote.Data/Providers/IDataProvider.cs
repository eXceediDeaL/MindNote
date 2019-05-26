using System.Text;

namespace MindNote.Data.Providers
{
    public interface IDataProvider
    {
        INodesProvider GetNodesProvider();

        IStructsProvider GetStructsProvider();

        ITagsProvider GetTagsProvider();

        IRelationsProvider GetRelationsProvider();
    }
}
