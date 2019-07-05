using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface ICategoriesProvider : IItemsProvider<Category>
    {
        Task<IEnumerable<Category>> Query(int? id, string name, string color, string userId, string identity);
    }
}
