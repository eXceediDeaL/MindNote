using System.Threading.Tasks;
using MindNote.Data.Mutations;
using MindNote.Data.Raws;
using MindNote.Frontend.SDK.API.Models;

namespace MindNote.Frontend.SDK.API
{
    public interface INotesClient
    {
        Task<bool> Clear();

        Task<int?> Create(RawNote data);

        Task<int?> Delete(int id);

        Task<Note> Get(int id);

        Task<PagingEnumerable<Note>> Query(int? id = null, string title = null, string content = null, int? categoryId = null, string keyword = null, ItemClass? itemClass = null, string userId = null, int? first = null, int? last = null, string before = null, string after = null);

        Task<int?> Update(int id, MutationNote mutation);
    }
}