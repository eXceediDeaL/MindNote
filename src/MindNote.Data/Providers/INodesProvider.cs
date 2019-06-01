using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface INodesProvider : IItemsProvider<Node>
    {
        Task<IEnumerable<Node>> Query(int? id, string name, string content, string userId = null);
    }
}
