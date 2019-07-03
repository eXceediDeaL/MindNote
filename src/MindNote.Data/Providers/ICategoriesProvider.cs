using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface ICategoriesProvider : IItemsProvider<Category>
    {
        Task<Category> GetByName(string name, string userId = null);

        Task<IEnumerable<Category>> Query(int? id, string name, string color, string userId = null);
    }
}
