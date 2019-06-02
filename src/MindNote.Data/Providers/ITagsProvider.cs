using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface ITagsProvider : IItemsProvider<Tag>
    {
        Task<Tag> GetByName(string name, string userId = null);

        Task<IEnumerable<Tag>> Query(int? id, string name, string color, string userId = null);
    }
}
