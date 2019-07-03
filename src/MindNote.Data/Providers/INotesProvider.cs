using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface INotesProvider : IItemsProvider<Note>
    {
        Task<IEnumerable<Note>> Query(int? id, string title, string content, int? categoryId, string keyword = null, string userId = null);
    }
}
