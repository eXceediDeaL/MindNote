using System.Threading.Tasks;
using MindNote.Data.Mutations;
using MindNote.Data.Raws;
using MindNote.Frontend.SDK.API.Models;

namespace MindNote.Frontend.SDK.API
{
    public interface ICategoriesClient
    {
        Task<bool> Clear();

        Task<int?> Create(RawCategory data);

        Task<int?> Delete(int id);

        Task<Category> Get(int id);

        Task<PagingEnumerable<Category>> Query(int? id = null, string name = null, string color = null, string userId = null, int? first = null, int? last = null, string before = null, string after = null);

        Task<int?> Update(int id, MutationCategory mutation);
    }
}