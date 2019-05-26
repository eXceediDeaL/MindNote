using System.Text;

namespace MindNote.Data.Providers
{
    public interface IDataProvider
    {
        INodesProvider GetNodesProvider();

        IStructsProvider GetStructsProvider();
    }
}
